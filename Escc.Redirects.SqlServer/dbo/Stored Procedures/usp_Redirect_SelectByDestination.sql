-- =============================================
-- Author:		Gordon Saxby
-- ALTER  date: 16 November 2015
-- Description:	Look for redirects matching the supplied destination
-- =============================================
CREATE PROCEDURE [dbo].[usp_Redirect_SelectByDestination] 
	@destination varchar(400)
AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;

    -- Sort table to ensure we get a consistent result, and process preferred URLs before older ones
	SELECT [RedirectId],[Pattern],[Destination],[Type],[Comment],[DateCreated]
    FROM Redirect
	WHERE 
		(Type = 1 AND LOWER(@destination) = LOWER(Destination)) OR -- Match short URL
		(Type = 2 AND RIGHT(LOWER(Destination), LEN(@destination)) = LOWER(@destination)) -- Ends with destination
	ORDER BY Type, LEN(Destination) DESC
END
GO
GRANT EXECUTE
    ON OBJECT::[dbo].[usp_Redirect_SelectByDestination] TO [Escc.Redirects.Reader]
    AS [dbo];

