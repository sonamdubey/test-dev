IF EXISTS (
    SELECT * FROM sysobjects WHERE id = object_id(N'[dbo].[Update_ClassifiedLeads]') 
    AND xtype IN (N'P')
)
    DROP PROCEDURE [dbo].[Update_ClassifiedLeads]
GO

	-- =============================================
-- Author:		Prachi Phalak
-- Create date: 10th sep, 2015
-- Description:	Updates a column 'IsSentToAutoBiz' in ClassifiedLeads table
-- =============================================
CREATE PROCEDURE [dbo].[Update_ClassifiedLeads]
	-- Add the parameters for the stored procedure here
	@Id As int
AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;

    -- Insert statements for procedure here
	UPDATE ClassifiedLeads SET IsSentToAutoBiz = 1 where Id = @Id

END
