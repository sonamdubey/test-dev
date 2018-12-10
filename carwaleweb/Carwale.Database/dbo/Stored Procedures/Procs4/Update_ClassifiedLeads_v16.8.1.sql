IF EXISTS (
    SELECT * FROM sysobjects WHERE id = object_id(N'[dbo].[Update_ClassifiedLeads_v16]') 
    AND xtype IN (N'P')
)
    DROP PROCEDURE [dbo].[Update_ClassifiedLeads_v16]
GO

	-- =============================================
-- Author:		Prachi Phalak
-- Create date: 10th sep, 2015
-- Description:	Updates a column 'IsSentToSource' in ClassifiedLeads table
-- Modified by Shubham Agarwal on 03/08/2016: changed column name IsSentToAutobiz to IsSentToSource
-- =============================================
CREATE PROCEDURE [dbo].[Update_ClassifiedLeads_v16.8.1]
	-- Add the parameters for the stored procedure here
	@Id As int
AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;

    -- Insert statements for procedure here
	UPDATE ClassifiedLeads SET IsSentToSource = 1 where Id = @Id

END

