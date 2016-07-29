-- =============================================
-- Author:		Rick Mason, Web Team
-- Create date: 9 Feb 2012
-- Description:	Read details of a redirect
-- =============================================
CREATE PROCEDURE [dbo].[usp_Redirect_Select]
	@redirectId int
AS
BEGIN
	SET NOCOUNT ON;
	SELECT RedirectId, Pattern, Destination, Type, Comment FROM Redirect WHERE RedirectId = @redirectId
END
GO
GRANT EXECUTE
    ON OBJECT::[dbo].[usp_Redirect_Select] TO [Escc.Redirects.Writer]
    AS [dbo];

