IF EXISTS (
    SELECT * FROM sysobjects WHERE id = object_id(N'[dbo].[TC_GetCarName_SP]') 
    AND xtype IN (N'P')
)
    DROP PROCEDURE [dbo].[TC_GetCarName_SP]
GO

	

---------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------
-- =============================================
-- Modified By:	Surendra
-- Create date: 05-10-2011
-- Description:	Procedure modified for checking valid querystring
-- =============================================
-- =============================================
-- Author:		Binumon George
-- Create date: 30-09-2011
-- Description:	Get car name,model, version, and registered place
-- =============================================
CREATE PROCEDURE [dbo].[TC_GetCarName_SP] 
	@StockId INT,
	@CarName VARCHAR(100) OUTPUT,
	@DealerId INT
AS
BEGIN
	IF EXISTS(SELECT * FROM TC_Stock WHERE Id=@StockId AND BranchId=@DealerId AND IsActive=1)
	BEGIN
		SELECT @CarName=( Ma.Name + ' ' + Mo.Name + ' ' + Ve.Name  +', '+UPPER(Cc.RegistrationPlace))
		FROM TC_Stock st
		INNER JOIN TC_StockStatus Tcs On Tcs.Id = St.StatusId
		LEFT JOIN CarVersions Ve On Ve.Id=St.VersionId 
		LEFT JOIN CarModels Mo On Mo.Id=Ve.CarModelId 
		LEFT JOIN CarMakes Ma On Ma.Id=Mo.CarMakeId 
		LEFT JOIN  TC_CarCondition Cc on Cc.StockId=@StockId
		WHERE st.Id=@StockId
		RETURN 0
	END
	ELSE
	BEGIN
		RETURN -1
	END
END







