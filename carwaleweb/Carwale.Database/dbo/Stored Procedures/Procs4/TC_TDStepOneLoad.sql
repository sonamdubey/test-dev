IF EXISTS (
    SELECT * FROM sysobjects WHERE id = object_id(N'[dbo].[TC_TDStepOneLoad]') 
    AND xtype IN (N'P')
)
    DROP PROCEDURE [dbo].[TC_TDStepOneLoad]
GO

	
--Create by: Binu
--Date:20,jun 2012
--Desc: To get area and source in first step 
CREATE PROCEDURE [dbo].[TC_TDStepOneLoad]
@BranchId BIGINT
AS
BEGIN 
	EXECUTE TC_GetAreas @BranchId=@BranchId
	EXECUTE TC_InquirySourceSelect		
END
