-- =============================================
-- Author:		Rick Mason, Web Team
-- Create date: 9 Feb 2012
-- Description:	Update a redirect
-- =============================================
CREATE PROCEDURE [dbo].[usp_Redirect_Update]
	@redirectId int,
	@pattern varchar(200),
	@destination varchar(400),
	@type int,
	@comment varchar(200)
AS
BEGIN
	SET @pattern = LTRIM(RTRIM(@pattern))
	SET @destination = LTRIM(RTRIM(@destination))

	IF LEFT(@pattern,1) = '/' SET @pattern = RIGHT(@pattern, LEN(@pattern)-1)

	UPDATE Redirect SET 
		Pattern = @pattern,
		Destination = @destination, 
		Type = @type, 
		Comment = @comment
	WHERE RedirectId = @redirectId
END
GO
GRANT EXECUTE
    ON OBJECT::[dbo].[usp_Redirect_Update] TO [Escc.Redirects.Writer]
    AS [dbo];

