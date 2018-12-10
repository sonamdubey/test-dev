IF EXISTS (
    SELECT * FROM sysobjects WHERE id = object_id(N'[dbo].[Microsite_DealerImageDelete]') 
    AND xtype IN (N'P')
)
    DROP PROCEDURE [dbo].[Microsite_DealerImageDelete]
GO

	-- =============================================
-- Author:		Chetan Kane
-- Create date: 27/3/2012
-- Description:	To Delete specific Dealer offers whos id is passed
-- =============================================
CREATE PROCEDURE [dbo].[Microsite_DealerImageDelete]
(  
 @Id INT
) 	
AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;

    -- Insert statements for procedure here
	UPDATE  Microsite_Images SET IsActive = 0  
	WHERE Id = @Id
	RETURN 1
END
