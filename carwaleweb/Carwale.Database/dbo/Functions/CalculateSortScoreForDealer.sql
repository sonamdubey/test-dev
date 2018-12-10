IF EXISTS (
    SELECT * FROM sysobjects WHERE id = object_id(N'[dbo].[CalculateSortScoreForDealer]') 
    AND xtype IN (N'FN', N'IF', N'TF')
)
    DROP FUNCTION [dbo].[CalculateSortScoreForDealer]
GO

	-- =============================================
-- Author:		Supriya Bhide
-- Create date: 24 June 2016
-- Description:	Calculates sortscore for a dealer car with given params
-- Modified by Navead Kazi on 30-08-2016 Removed the cases for +ve premium carscore check and asign bucket 10
-- Modified by Navead Kazi on 19-09-2016 Inorder to push up optimizer stock,we removed the cases for +ve maximizerPlus and Maximizer carscore check and assigned them to buckets 9 and 8 respectively.
--Modified by Navead Kazi/Kinzal Ostwal on 21-09-2016 Inorder to push +ve premium good stock score >=0.46 up and assign bucket (15)
-- Modified by Sahil & Afrose on 27-09-2016, changed sort score logic for -ve dealers, now calculating bucket based on customer leads and car score

-- =============================================
CREATE FUNCTION [dbo].[CalculateSortScoreForDealer]
(
	@PackageId INT,
	@SVScore FLOAT,
	@CarScore FLOAT,
	@NewScore FLOAT,
	@PhotoCount INT,
	@CustCount INT
)
RETURNS FLOAT
AS
BEGIN
	-- Declare the return variable here
	DECLARE @minCustCountForDealer INT = 40,
			@IsEligibleForBoost INT = 0,
			@maxBucketValue INT = 15,
			@SortScoreNew FLOAT,
			@SortBucket TINYINT

			
	SET @IsEligibleForBoost = CASE 
								WHEN @SVScore < 0 OR (@SVScore >= 0 AND @CustCount <= @minCustCountForDealer) THEN 1
								ELSE 0 
							  END

	IF (@IsEligibleForBoost = 0) 
	BEGIN 
		SELECT @SortBucket= CASE WHEN @CustCount >= CustCountThreshold
		                         THEN SortBucketDown	
								 ELSE SortBucketUp END						
				    		FROM UsedSortBucketConfig WITH(NOLOCK) 
							WHERE PackageId=@PackageId and @Carscore > CarScoreLowerLimit and @Carscore<=CarScoreUpperLimit						
	END
									 

	-- Add the T-SQL statements to compute the return value here
	SET @SortScoreNew =
		(CASE(@PackageId)
			WHEN 81 THEN 	--Dealer Premium Pan-India
				(CASE 
					WHEN @IsEligibleForBoost = 1 THEN 15
					WHEN @IsEligibleForBoost = 0 THEN @SortBucket			
						
						
				END)
			WHEN 47 THEN 	--Dealer Premium
				(CASE 
					WHEN @IsEligibleForBoost = 1 THEN 15     
					WHEN @IsEligibleForBoost = 0 THEN @SortBucket
				END)
			WHEN 98 THEN	--Maximizer Plus
				(CASE
					WHEN @IsEligibleForBoost = 1 THEN
						(CASE
							WHEN @CarScore < 0.2 THEN 14
							WHEN @CarScore > 0.35 THEN 15
							WHEN @CarScore BETWEEN 0.2 AND 0.35 THEN 14
						END)
					WHEN @IsEligibleForBoost = 0 THEN @SortBucket 
					END)
			WHEN 32 THEN	--Maximizer
				(CASE
					WHEN @IsEligibleForBoost = 1 THEN
						(CASE
							WHEN @CarScore < 0.2 THEN 13
							WHEN @CarScore > 0.35 THEN 15
							WHEN @CarScore BETWEEN 0.2 AND 0.35 THEN 14
						END)
					WHEN @IsEligibleForBoost = 0 THEN @SortBucket
					END)							
			WHEN 30 THEN	--Optimizer
				(CASE
					WHEN @IsEligibleForBoost = 1 THEN
						(CASE
							WHEN @CarScore < 0.2 THEN 12
							WHEN @CarScore > 0.35 THEN 15
							WHEN @CarScore BETWEEN 0.2 AND 0.35 THEN 14
						END)
					WHEN @IsEligibleForBoost = 0 THEN @SortBucket
						
				END)
			WHEN 34 THEN	--Starter
				(CASE
					WHEN @IsEligibleForBoost = 1 THEN
						(CASE
							WHEN @CarScore < 0.2 THEN 11
							WHEN @CarScore > 0.35 THEN 15
							WHEN @CarScore BETWEEN 0.2 AND 0.35 THEN 14
						END)
					WHEN @IsEligibleForBoost = 0 THEN @SortBucket
				END)
			WHEN 77 THEN 5 --CPS Paid
			WHEN 76 THEN 4 --CPS Free
			WHEN 46 THEN 2 --Free Dealer
		END) + ISNULL(@NewScore,ABS(@CarScore) - FLOOR(ABS(@CarScore)))
				+ CASE WHEN @PhotoCount > 0 THEN 0 ELSE -@maxBucketValue END

	-- Return the result of the function
	--SELECT @SortScoreNew
	RETURN @SortScoreNew

END
