IF EXISTS (
    SELECT * FROM sysobjects WHERE id = object_id(N'[dbo].[TC_MMCheckIfAlreadyProcessing]') 
    AND xtype IN (N'P')
)
    DROP PROCEDURE [dbo].[TC_MMCheckIfAlreadyProcessing]
GO

	
-- =============================================
-- Author:		<Author, Nilesh Utture>
-- Create date: <Create Date, 09-10-2013>
-- Description:	<Description, Check if stockId's for particular DealerId and pageId whether if being processed>
--				<CASE: Processing THEN Return 1 ELSE RETURN 0>
-- =============================================
CREATE PROCEDURE [dbo].[TC_MMCheckIfAlreadyProcessing]
	-- Add the parameters for the stored procedure here
	@BranchId INT,
	@pageId TINYINT

AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;
	
	SELECT	DealerId 
	FROM	TC_MMExecutionFlg 
	WHERE	DealerId = @BranchId AND 
			PageId = @pageId

	IF(@@ROWCOUNT = 0)
	BEGIN
		RETURN 0
	END

	RETURN 1
END
