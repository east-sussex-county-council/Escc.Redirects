
-- =============================================
-- Author:		Rick Mason, Web Team
-- ALTER  date: 14 April 2011
-- Description:	Look for a redirect matching the current request
-- =============================================
CREATE PROCEDURE [dbo].[usp_Redirect_MatchRequest]
	@request varchar(200)
AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;

    -- Sort table to ensure we get a consistent result, and process preferred URLs before older ones
	SELECT TOP 1 RedirectId, Destination, Type FROM Redirect
	WHERE 
		(Type = 1 AND LOWER(@request) = LOWER(Pattern)) OR -- Match short URL
		(Type = 2 AND ((LEFT(LOWER(@request), LEN(Pattern)) = LOWER(Pattern)) OR
					   (LEFT(LOWER(@request) + '/', LEN(Pattern)) = LOWER(Pattern)))) -- Starts with redirect URL
	ORDER BY Type, LEN(Pattern) DESC
END
GO
GRANT EXECUTE
    ON OBJECT::[dbo].[usp_Redirect_MatchRequest] TO [Escc.Redirects.Reader]
    AS [dbo];

