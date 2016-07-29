-- =============================================
-- Author:		Rick Mason, Web Team
-- Create date: 19 Sept 2012
-- Description:	Find the nearest short URL to a given page
-- =============================================
CREATE PROCEDURE usp_Redirect_SelectBestMatch
	@urlAbsolutePath varchar(250)
AS
BEGIN
	-- Create table variable to select into, so we don't return multiple result sets while trying different SELECTs
	DECLARE @results TABLE (ShortUrl varchar(200))

	-- Look for an exact match for the current page
	INSERT INTO @results SELECT pattern FROM Redirect WHERE type = 1 AND destination = @urlAbsolutePath
	IF @@ROWCOUNT > 0 GOTO ReturnResults

	-- Cut back URL path a segment at a time
	TryFolder:

	-- Remove page name or last folder tried based on position of last slash
	SELECT @urlAbsolutePath = LEFT(@urlAbsolutePath, LEN(@urlAbsolutePath) - CHARINDEX('/', REVERSE(@urlAbsolutePath)) + 1) 

	-- Stop when the URL has been reduced to / or an empty string
	IF LEN(@urlAbsolutePath) <= 1 GOTO ReturnResults

	-- Look again for exact match based on the folder URL alone
	INSERT INTO @results SELECT pattern FROM Redirect WHERE type = 1 AND destination = @urlAbsolutePath
	IF @@ROWCOUNT > 0 GOTO ReturnResults

	-- Look for matching folder with standard home page name
	INSERT INTO @results SELECT pattern FROM Redirect WHERE type = 1 AND destination LIKE @urlAbsolutePath + 'default.htm'
	IF @@ROWCOUNT > 0 GOTO ReturnResults

	-- Look for matching folder with a different filename
	INSERT INTO @results SELECT pattern FROM Redirect WHERE type = 1 AND (destination LIKE @urlAbsolutePath + '%' AND destination NOT LIKE @urlAbsolutePath + '%/%')
	IF @@ROWCOUNT > 0 GOTO ReturnResults

	-- Cut off trailing slash and repeat
	SELECT @urlAbsolutePath = LEFT(@urlAbsolutePath, LEN(@urlAbsolutePath)-1)

	GOTO TryFolder

	-- Return results, whether that's no matches or several
	ReturnResults:
	SELECT 'http://www.eastsussex.gov.uk/' + ShortUrl AS ShortUrl FROM @results
END
GO
GRANT EXECUTE
    ON OBJECT::[dbo].[usp_Redirect_SelectBestMatch] TO [Escc.Redirects.Reader]
    AS [dbo];

