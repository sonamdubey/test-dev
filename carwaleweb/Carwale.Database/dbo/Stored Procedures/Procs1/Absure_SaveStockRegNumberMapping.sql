IF EXISTS (
    SELECT * FROM sysobjects WHERE id = object_id(N'[dbo].[Absure_SaveStockRegNumberMapping]') 
    AND xtype IN (N'P')
)
    DROP PROCEDURE [dbo].[Absure_SaveStockRegNumberMapping]
GO

	
-- =============================================
-- Author: KARTIK RATHOD
-- Create date: 14 Sept 2015
-- Description:  Save Stock Id And Reg Number for Mapping Car and update carid in CarDeatails table only for duplicate car. 
--				 Update Tc_Stock And Absure_CarDetails Table
--				(One can go ahead with mapping if only @IsCarMapped and @IsStockMapped both are FALSE(0).)
-- Modified By : Ruchira Patil 19th Oct 2015 (@AutoLinking is added where @AutoLinking=1)
-- =============================================  

CREATE PROC [dbo].[Absure_SaveStockRegNumberMapping]
@StockId INT,--(car to which the car is to be mapped with)
@RegNumber VARCHAR(50),
@CarId INT, -- car which is to be mapped(having stockid null)
@DealerId INT,
@AutoLinking BIT = NULL
AS
BEGIN
	DECLARE @IsCarMapped BIT = 0 ,               -- use to validate the the provided carid is already mapped or not 
			@IsStockMapped BIT = 0 				-- use to validate the the provided stockid is already mapped or not 
												

	DECLARE @CancelledReasonId	TINYINT = 7, 
			@CancelledReason	VARCHAR(250),
			@IsDuplicateCar BIT 

	SELECT  @CancelledReason = Reason
	FROM	AbSure_ReqCancellationReason WITH (NOLOCK)
	WHERE	Id = @CancelledReasonId;

	/* #TblTemp is being used to validate if the given stockid is already mapped or not  */
			WITH CTE
			AS(			
			SELECT	ST.Id StockId
			FROM	TC_Stock ST WITH (NOLOCK)
					INNER JOIN vwAllMMV V WITH (NOLOCK) ON V.VersionId = ST.VersionId AND V.ApplicationId = 1
					INNER JOIN AbSure_EligibleModels AE WITH(NOLOCK) ON AE.ModelId = V.ModelId
			WHERE	ST.BranchId = @DealerId
					AND ST.IsActive = 1
					AND (ST.IsWarrantyRequested = 0 OR ST.IsWarrantyRequested IS NULL)
					AND ST.StatusId = 1 
					AND ISNULL(AE.IsEligibleWarranty,0) = 1
					AND AE.IsActive = 1
			UNION
			SELECT	CD.StockId StockId
			FROM	AbSure_CarDetails CD WITH (NOLOCK)
					INNER JOIN TC_Stock ST WITH (NOLOCK) ON CD.StockId = ST.Id  
					INNER JOIN vwAllMMV V WITH (NOLOCK) ON V.VersionId = ST.VersionId AND V.ApplicationId = 1 
					INNER JOIN AbSure_EligibleModels AE WITH(NOLOCK) ON AE.ModelId = V.ModelId
			WHERE	CD.StockId IS NOT NULL 
					AND CD.DealerId = @DealerId
					AND (
						 ((CD.Status IN (1,2,9) OR (CD.Status=4 AND CD.IsSoldOut = 0 )) AND DATEDIFF(DAY,CD.SurveyDate,GETDATE()) >30)
						 OR
						 (CD.Status IN (5,6))
						 OR
						 (CD.Status = 3 AND CD.CancelReason <> @CancelledReason) 
						)
					AND CD.IsActive = 1
					AND ISNULL(AE.IsEligibleWarranty,0) = 1    
					AND AE.IsActive = 1 
					AND ST.StatusId = 1            
			)
	
			SELECT StockId INTO #TblTemp
			FROM CTE WITH(NOLOCK)
	
	IF (SELECT StockId FROM #TblTemp WITH(NOLOCK) WHERE StockId=@StockId) IS NULL AND @AutoLinking IS NULL     --if fetched stockid is null then that perticular @StockId is already mapped ie.@IsStockMapped = 1
		SET @IsStockMapped = 1																					
	ELSE 
		SET @IsStockMapped = 0																				-- autolinking is directly allowed 
	
	IF (SELECT StockId FROM AbSure_CarDetails WITH(NOLOCK) WHERE Id=@CarId) IS NOT NULL	AND @AutoLinking IS NULL	 --if fetched stockid is not null and not mapped then that perticular @CarId is already mapped
		SET @IsCarMapped = 1																						
	ELSE 
		SET @IsCarMapped = 0																						-- autolinking is directly allowed 

	IF(@IsCarMapped = 0 AND @IsStockMapped = 0)					-- if the carid and stockid is not mapped then only the Absure_CarDetails And TC_Stock table is going to update  
	BEGIN																					
		
		SELECT @IsDuplicateCar = CASE WHEN Status = 3 AND CancelReason = @CancelledReason THEN 1 ELSE 0 END 
		FROM  AbSure_CarDetails WITH(NOLOCK)
		WHERE ID = @CarId

		UPDATE AbSure_CarDetails							-- Modified By Kartik rathod, MAke IsActive = 0 for all the previous entries in Absure_CarDetails for provided StockId
		SET IsActive = 0
		WHERE StockId = @StockId

		IF(@IsDuplicateCar = 1)
		BEGIN
		INSERT INTO 
		Absure_StockRegNumberMapping(TC_StockId,RegistrationNumber, EntryDate,IsActive)
		VALUES(@StockId, @RegNumber,GETDATE(), 1)
		END
	
		UPDATE	AbSure_CarDetails															
		SET		StockId = @StockId,IsActive = 1 
		WHERE	Id = @CarId

		UPDATE	TC_Stock																
		SET		IsWarrantyRequested =1 
		WHERE	ID = @StockId
	END
	
	SELECT @IsCarMapped IsCarMapped ,@IsStockMapped IsStockMapped

END
