IF EXISTS (
    SELECT * FROM sysobjects WHERE id = object_id(N'[dbo].[TC_TDGridViewOnLoad]') 
    AND xtype IN (N'P')
)
    DROP PROCEDURE [dbo].[TC_TDGridViewOnLoad]
GO

	-- =============================================
-- Author:        Tejashree Patil
-- Create date: 17 July 2012
-- Description:    To get Area,TdCars,TdConsultants in on load
-- =============================================
CREATE PROCEDURE [dbo].[TC_TDGridViewOnLoad]
    -- Add the parameters for the stored procedure here
    @BranchId BIGINT
AS
BEGIN
    EXECUTE TC_GetAreas @BranchId=@BranchId
    EXECUTE TC_GetTestDriveCars @BranchId=@BranchId
    EXECUTE TC_TDConsultant @BranchId=@BranchId    
END