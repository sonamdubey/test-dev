IF EXISTS (
    SELECT * FROM sysobjects WHERE id = object_id(N'[dbo].[TC_DeleteSentMMLeadsMFC]') 
    AND xtype IN (N'P')
)
    DROP PROCEDURE [dbo].[TC_DeleteSentMMLeadsMFC]
GO

	-- =============================================
-- Author:		Vivek Gupta
-- Create date: 18-02-2015
-- Description:	delete sent leads from TC_MixMatchLeadsMFC
-- =============================================
CREATE PROCEDURE [dbo].[TC_DeleteSentMMLeadsMFC]
	@RcdsToDelete VARCHAR(MAX)
AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;

	DELETE 
	FROM TC_MixMatchLeadsMFC 
	WHERE ID IN (SELECT ListMember FROM fnSplitCSV(@RcdsToDelete))
    
END
