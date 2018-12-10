IF EXISTS (
    SELECT * FROM sysobjects WHERE id = object_id(N'[dbo].[Absure_GetMasterCarId]') 
    AND xtype IN (N'FN', N'IF', N'TF')
)
    DROP FUNCTION [dbo].[Absure_GetMasterCarId]
GO

	-- =============================================
-- Author:		Tejashree Patil	
-- Create date: 10 September 2015
-- Description:	To get appropriate mapped carid based on stockId.
-- SELECT dbo.Absure_GetMasterCarId(null,138)
-- Modified by : Kartik Rathod on 21 Sept 2015, Added @Carid on the basis of the RegNumber.  
-- Modifed By : Ashwini Dhamankar on Sep 30,2015 (Added @RegNumber as parameter and changed logic accordingly)
-- =============================================
CREATE FUNCTION [dbo].[Absure_GetMasterCarId]
(
	--@StockId				BIGINT = NULL,
	@RegNumber VARCHAR(50) = NULL,
	@AbSure_CarDetailsId	BIGINT = NULL
)
RETURNS BIGINT
AS
BEGIN

	DECLARE --@RegNumber VARCHAR(50) = NULL,
        @MappedRegNumber VARCHAR(50) = NULL,
        @RetCarId BIGINT,
        @CarId BIGINT

--IF(@StockId IS NULL AND @AbSure_CarDetailsId IS NOT NULL)
--BEGIN
	--SELECT @RegNumber = RegNumber 
	--FROM  AbSure_CarDetails WITH(NOLOCK)
	--WHERE Id = @AbSure_CarDetailsId OR StockId = @StockId
--END

	SELECT		TOP 1 @MappedRegNumber =  RegistrationNumber 
	FROM		AbSure_StockRegNumberMapping WITH(NOLOCK)
	WHERE		RegistrationNumber = @RegNumber  
	ORDER BY	EntryDate DESC

	SELECT		TOP 1 @RetCarId = CD.Id
	FROM		AbSure_CarDetails CD WITH(NOLOCK)
	WHERE		(CD.RegNumber = @MappedRegNumber AND CD.IsSurveyDone = 1 AND CD.IsActive = 1) 	
	ORDER BY	SurveyDate DESC

IF(@MappedRegNumber IS NOT NULL)           --If Stock Linked to Car i.e. If Duplicate car is added, get origional carId which was inspected based on requested registration number.
BEGIN
		SELECT @RetCarId = @RetCarId
		--SELECT		TOP 1 @RetCarId = CD.Id
		--FROM		AbSure_CarDetails CD WITH(NOLOCK)
		--WHERE		(CD.RegNumber = @MappedRegNumber AND CD.IsSurveyDone = 1 AND CD.IsActive = 1)
		--ORDER BY	SurveyDate DESC
END
ELSE										--If Stock not Linked to Car i.e. If there is no duplicate car, get origional carId which was inspected based on requested stockId or carId.
BEGIN
		SELECT      TOP 1 @RetCarId = CD.Id 
        FROM        AbSure_CarDetails CD WITH(NOLOCK)
        WHERE       CD.RegNumber = @RegNumber
                    AND CD.IsActive = 1
					and IsSurveyDone=1 --and SurveyDate is not null--ADDED FOR TESTING by SHRUTI>>>>>>>>>>>>>
        ORDER BY    CD.SurveyDate DESC

		--SELECT      TOP 1 @CarId = CD.Id
  --      FROM        AbSure_CarDetails CD WITH(NOLOCK)
  --      WHERE       CD.RegNumber = @RegNumber 
  --                  AND CD.IsActive = 1
  --      ORDER BY    CD.SurveyDate DESC
		
		--SELECT      TOP 1 @RetCarId = CD.Id
  --      FROM		AbSure_CarDetails CD WITH(NOLOCK)
  --      WHERE       CD.StockId = @StockId OR CD.Id = @CarId
END
IF (@RetCarId IS NULL)
BEGIN
SET @RetCarId = @AbSure_CarDetailsId
END

RETURN @RetCarId

END
