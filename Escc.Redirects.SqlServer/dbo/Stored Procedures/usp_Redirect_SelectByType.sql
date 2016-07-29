-- =============================================
-- Author:		Rick Mason, Web Team
-- Create date: 15 April 2011
-- Description:	Select all redirects of a given type
-- =============================================
CREATE PROCEDURE [dbo].[usp_Redirect_SelectByType]
	@type int,
	@sort varchar(15) 
AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;

IF @sort = 'pattern'
	SELECT RedirectId, Pattern, Destination, Comment FROM Redirect 
	WHERE Type = @type
	ORDER BY Pattern
ELSE IF @sort = 'destination'
	SELECT RedirectId, Pattern, Destination, Comment FROM Redirect 
	WHERE Type = @type
	ORDER BY Destination
ELSE
	SELECT RedirectId, Pattern, Destination, Comment FROM Redirect 
	WHERE Type = @type
	ORDER BY DateCreated DESC

END
GO
GRANT EXECUTE
    ON OBJECT::[dbo].[usp_Redirect_SelectByType] TO [Escc.Redirects.Reader]
    AS [dbo];

