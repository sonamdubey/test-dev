IF EXISTS (
    SELECT * FROM sysobjects WHERE id = object_id(N'[dbo].[TC_AlertsInsert]') 
    AND xtype IN (N'P')
)
    DROP PROCEDURE [dbo].[TC_AlertsInsert]
GO

	
-- =============================================
-- Author:		Surendra Chouksey
-- Create date: 5th Dec,2011
-- Description:	This procedure will insert record TC_Alerts that is called in DCRM to ACCESS the Trading Cars
-- =============================================
CREATE PROCEDURE [dbo].[TC_AlertsInsert]
(
@BranchId NUMERIC
)	
AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	SET NOCOUNT ON;
		
	IF NOT EXISTS(SELECT TC_Alerts_id FROM TC_Alerts WHERE BranchId=@BranchId AND Status IS NULL)
	BEGIN
		INSERT INTO TC_Alerts(BranchId,AlertType_Id)
		VALUES(@BranchId,1)
		RETURN 0
	END 
	ELSE
	BEGIN
		RETURN -1
	END   
END


