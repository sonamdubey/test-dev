IF EXISTS (
    SELECT * FROM sysobjects WHERE id = object_id(N'[dbo].[TC_APIDrilDownDealers]') 
    AND xtype IN (N'P')
)
    DROP PROCEDURE [dbo].[TC_APIDrilDownDealers]
GO

	-- =============================================
-- Author:		VISHAL SRIVASTAVA AE1830	
-- Create date: 11-04-2014 1010 HRS IST
-- Description:	mOBILE aPI dRILL dOWN fOR dEALERS
-- =============================================
CREATE PROCEDURE [dbo].[TC_APIDrilDownDealers]
	-- Add the parameters for the stored procedure here
	@UserId INT
AS
BEGIN
	DECLARE @HierId HIERARCHYID
	DECLARE @LVL SMALLINT
	DECLARE @BranchId SMALLINT

		SELECT  @HierId =hierid ,@LVL=LVL,@BranchId=BranchId FROM TC_Users WHERE id=@UserId
	
		SELECT u.Id as UserId,
		       u.UserName,
			   'False' AS SpecialUser
		FROM TC_Users as u
    	WHERE u.Hierid.IsDescendantOf(@HierId)= 1
		AND u.lvl=@lvl+1
		AND u.BranchId=@BranchId
		AND u.IsActive=1
		ORDER BY UserId
END
