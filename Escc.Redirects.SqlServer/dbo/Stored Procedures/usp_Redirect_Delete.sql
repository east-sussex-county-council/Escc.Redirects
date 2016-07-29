-- =============================================
-- Author:		Rick Mason, Web Team
-- Create date: 9 Feb 2012
-- Description:	Delete a short URL
-- =============================================
CREATE PROCEDURE [dbo].[usp_Redirect_Delete]
	@redirectId int
AS
BEGIN
	DELETE FROM Redirect WHERE RedirectId = @redirectId
END
GO
GRANT EXECUTE
    ON OBJECT::[dbo].[usp_Redirect_Delete] TO [Escc.Redirects.Writer]
    AS [dbo];

