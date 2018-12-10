IF EXISTS (
    SELECT * FROM sysobjects WHERE id = object_id(N'[CRM].[LSUpdateLeadScore]') 
    AND xtype IN (N'P')
)
    DROP PROCEDURE [CRM].[LSUpdateLeadScore]
GO

	


-- Description	:	Update/Calculate the score of the lead as per the request
-- Author		:	Deepak Tripathi. 24-May-2012

CREATE PROCEDURE [CRM].[LSUpdateLeadScore]	
	@SectionId	INT,
	@LeadId		NUMERIC,
	@CustomerId	NUMERIC
AS
BEGIN
	
	DECLARE @LSCategoryId  INT
	DECLARE @EstimateVal FLOAT
	DECLARE @SubCategoryId INT
	DECLARE @CategoryLeadScore FLOAT
	DECLARE @NewLeadScore FLOAT
	DECLARE @PrevLeadScore FLOAT
	
	SET NOCOUNT ON;
	
	BEGIN		
		
		IF @SectionId = 1 -- Score lead whenever there is a new car addtion against the lead
			BEGIN
				IF @LeadId <> -1
					BEGIN
					--Count Total New Cars Added in this lead
						SET @LSCategoryId = 1
						DECLARE @TotalCars INT
						DECLARE @DistinctTotalCars INT
						
						SELECT @TotalCars = COUNT(Id), @DistinctTotalCars = COUNT(DISTINCT VersionId)
						FROM CRM_CarBasicData WITH (NOLOCK) WHERE LeadId = @LeadId

						IF @TotalCars > 0
							BEGIN
								--Cars Available
								SET @SubCategoryId = 1
								
								--Get the estimate value
								SELECT @EstimateVal = Value FROM CRM.LSSubCategory WITH (NOLOCK) WHERE SubCategoryId = @SubCategoryId AND IsActive = 1
																
								--Do other Operation
								SET @CategoryLeadScore = (@TotalCars * @EstimateVal)
								EXEC CRM.LSCalculateLeadScore @LeadId, @LSCategoryId, @SubCategoryId, @CategoryLeadScore
							END
							
					--Count Distinct Versions Added in this lead
						SET @LSCategoryId = 2
						SET @CategoryLeadScore = NULL
						IF @DistinctTotalCars > 0
							BEGIN
								--Cars Available
								SET @SubCategoryId = 2
								
								--Get the estimate value
								SELECT @EstimateVal = Value FROM CRM.LSSubCategory WITH (NOLOCK) WHERE SubCategoryId = @SubCategoryId AND IsActive = 1
																
								--Do other Operation
								SET @CategoryLeadScore = (@DistinctTotalCars * @EstimateVal)
								EXEC CRM.LSCalculateLeadScore @LeadId, @LSCategoryId, @SubCategoryId, @CategoryLeadScore
							END
							
					--Count Distinct Versions PQ in NewCarPurchaseInquiryTable
						--IF @CustomerId <> -1
						--	BEGIN
						--		SET @LSCategoryId = 16
						--		SET @CategoryLeadScore = NULL
						--		SET @DistinctTotalCars = NULL
								
						--		SELECT @DistinctTotalCars = COUNT(DISTINCT CarVersionId)
						--		FROM CRM_Customers AS CC, NewCarPurchaseInquiries AS NPI
						--		WHERE CC.CWCustId = NPI.CustomerId AND CC.Id = @CustomerId
						--		IF @DistinctTotalCars > 0
						--			BEGIN
						--				--Cars Available
						--				SET @SubCategoryId = 25
										
						--				--Get the estimate value
						--				SELECT @EstimateVal = Value FROM CRM.LSSubCategory WHERE SubCategoryId = @SubCategoryId AND IsActive = 1
																		
						--				--Do other Operation
						--				SET @CategoryLeadScore = (@DistinctTotalCars * @EstimateVal)
						--				EXEC CRM.LSCalculateLeadScore @LeadId, @LSCategoryId, @SubCategoryId, @CategoryLeadScore
						--			END
						--	END
							
					--Count Difference in dates(1st PQ and Last PQ)CarPurchaseInquiryTable
						--IF @CustomerId <> -1
						--	BEGIN
						--		SET @LSCategoryId = 17
						--		SET @CategoryLeadScore = NULL
						--		DECLARE @DayDiff BIGINT
								
						--		SELECT @DayDiff = DATEDIFF(DD, MIN(NPI.RequestDateTime), MAX(NPI.RequestDateTime))
						--		FROM CRM_Customers AS CC, NewCarPurchaseInquiries AS NPI
						--		WHERE CC.CWCustId = NPI.CustomerId AND CC.Id = @CustomerId
						--		IF @DayDiff > 0
						--			BEGIN
						--				--Cars Available
						--				SET @SubCategoryId = 26
										
						--				--Get the estimate value
						--				SELECT @EstimateVal = Value FROM CRM.LSSubCategory WHERE SubCategoryId = @SubCategoryId AND IsActive = 1
																		
						--				--Do other Operation
						--				SET @CategoryLeadScore = (@DayDiff * @EstimateVal)
						--				EXEC CRM.LSCalculateLeadScore @LeadId, @LSCategoryId, @SubCategoryId, @CategoryLeadScore
						--			END
						--	END
							
					--New Car Brands available in this car
						SET @LSCategoryId = 3
						SET @CategoryLeadScore = NULL
						
						DECLARE @TempPQ Table(MakeId NUMERIC, TPQ NUMERIC, Value FLOAT)
						
						-- Get All PQ Taken by Customer & corresponding estimate value if available
						INSERT INTO @TempPQ		
						SELECT VM.MakeId, COUNT(Id) TPQ, LSC.Value  
						FROM CRM_CarBasicData CBD WITH (NOLOCK), vwMMV AS VM WITH (NOLOCK) 
						LEFT JOIN CRM.LSSubCategory AS LSC WITH (NOLOCK) ON VM.MakeId = LSC.MakeId
						WHERE CBD.VersionId = VM.VersionId AND LeadId = @LeadId
						GROUP BY VM.MakeId, LSC.Value 
						
						SET @SubCategoryId = 6
						DECLARE @TBrandEstimate FLOAT
						DECLARE @TPQOthers INT
						
						--Select estimate value sum 
						SELECT @TBrandEstimate = SUM(ISNULL(Value,0)) FROM @TempPQ 
						
						--Select sum of count where estimate value is not given
						SELECT  @TPQOthers = SUM(TPQ)
						FROM @TempPQ WHERE Value IS NULL
						
						-- If other cars(Where estimate is not available) count is more than 0 then get the estimate for them
						IF @TPQOthers > 0
							BEGIN
								SET @SubCategoryId = 10
								SELECT @TBrandEstimate = @TBrandEstimate + (@TPQOthers * Value)
								FROM CRM.LSSubCategory WITH (NOLOCK) WHERE MakeId = -1
							END
							
						--Do other Operation
						SET @CategoryLeadScore = @TBrandEstimate
						EXEC CRM.LSCalculateLeadScore @LeadId, @LSCategoryId, @SubCategoryId, @CategoryLeadScore
						
					--Available cars Average ex-showroom price range
						SET @LSCategoryId = 4
						SET @CategoryLeadScore = NULL
						DECLARE @AvgPrice BIGINT
						SELECT @AvgPrice = AVG(NCP.Price)
						FROM CRM_CarBasicData CBD WITH (NOLOCK), NewCarShowroomPrices AS NCP WITH (NOLOCK)
						WHERE CBD.VersionId = NCP.CarVersionId AND NCP.CityId = CBD.CityId AND LeadId = @LeadId
						
						IF @AvgPrice > 0
							BEGIN
								--Cars Available
								SET @SubCategoryId = NULL
								
								--Get the estimate value
								SELECT @EstimateVal = Value, @SubCategoryId = SubCategoryId 
								FROM CRM.LSSubCategory WITH (NOLOCK) WHERE @AvgPrice BETWEEN PriceFrom AND PriceTo AND IsActive = 1
								
								IF @@ROWCOUNT > 0
									BEGIN								
										--Do other Operation
										SET @CategoryLeadScore =  @EstimateVal
										EXEC CRM.LSCalculateLeadScore @LeadId, @LSCategoryId, @SubCategoryId, @CategoryLeadScore
									END
							END
						
					END
				
			END
			
		ELSE IF @SectionId = 2 -- Score lead on the basis of buy time
			BEGIN
				
				IF @LeadId <> -1
					BEGIN
					--Lead Buy Time
						SET @LSCategoryId = 5
						DECLARE @TotalDays INT
						SELECT @TotalDays = PurchaseTime
						FROM CRM_VerificationOthersLog WITH (NOLOCK) WHERE LeadId = @LeadId 
						IF @TotalDays = 30 OR @TotalDays = 60
							BEGIN
								--Buy Time is given
								IF @TotalDays = 30
									SET @SubCategoryId = 11
								ELSE IF @TotalDays = 60
									SET @SubCategoryId = 12
								
								--Get the estimate value
								SELECT @EstimateVal = Value FROM CRM.LSSubCategory WITH (NOLOCK) WHERE SubCategoryId = @SubCategoryId AND IsActive = 1
																
								--Do other Operation
								SET @CategoryLeadScore =  @EstimateVal
								EXEC CRM.LSCalculateLeadScore @LeadId, @LSCategoryId, @SubCategoryId, @CategoryLeadScore
							END
					END		
			END
		
		ELSE IF @SectionId = 3 -- Score lead on the basis of lead source
			BEGIN
				IF @LeadId <> -1
					BEGIN
					--Lead Source
						SET @LSCategoryId = 6
						DECLARE @LeadSource AS INT
						DECLARE @LeadCategory AS INT
						SELECT @LeadSource = CL.SourceId, @LeadCategory = CL.CategoryId
						FROM CRM_LeadSource CL WITH (NOLOCK) WHERE CL.LeadId = @LeadId
						
						IF @@ROWCOUNT > 0
							BEGIN
								IF @LeadCategory = 3 -- Id Lead is from Agencies
									BEGIN
										--Record Available
										SET @SubCategoryId = NULL
										
										--Get the estimate value
										SELECT @EstimateVal = Value, @SubCategoryId = SubCategoryId 
										FROM CRM.LSSubCategory WITH (NOLOCK) WHERE SourceId = @LeadSource AND IsActive = 1
										
										IF @@ROWCOUNT > 0
											BEGIN								
												--Do other Operation
												SET @CategoryLeadScore =  @EstimateVal
												EXEC CRM.LSCalculateLeadScore @LeadId, @LSCategoryId, @SubCategoryId, @CategoryLeadScore
											END
									END
							END
					END	
			END
			
		ELSE IF @SectionId = 4 -- Score Lead When there is a PQ/PQ Completed/PQ Not Required
			
			BEGIN
				IF @LeadId <> -1
					BEGIN
					--PQ Requested
						SET @LSCategoryId = 9
						DECLARE @PQReq INT
						SELECT @PQReq = COUNT(DISTINCT VM.MakeId)   
						FROM CRM_CarBasicData CBD WITH (NOLOCK), CRM_CarPQLog AS CPQ WITH (NOLOCK), vwMMV AS VM WITH (NOLOCK)
						WHERE CBD.Id = CPQ.CBDId AND CBD.VersionId = VM.VersionId
						AND CPQ.IsPQRequested = 1 AND CBD.LeadId = @LeadId
						
						IF @PQReq > 0
							BEGIN
								--Record Available
								SET @SubCategoryId = 17
								
								--Get the estimate value
								SELECT @EstimateVal = Value
								FROM CRM.LSSubCategory WITH (NOLOCK) WHERE SubCategoryId = @SubCategoryId AND IsActive = 1
								
								IF @@ROWCOUNT > 0
									BEGIN								
										--Do other Operation
										SET @CategoryLeadScore =  (@PQReq * @EstimateVal)
										EXEC CRM.LSCalculateLeadScore @LeadId, @LSCategoryId, @SubCategoryId, @CategoryLeadScore
									END
							END
					--PQ Requested Body Style
						SET @LSCategoryId = 10
						SET @CategoryLeadScore = NULL
						DECLARE @PQReqBody INT
						SELECT @PQReqBody = COUNT(DISTINCT VM.BodyStyleId)   
						FROM CRM_CarBasicData CBD WITH (NOLOCK), CRM_CarPQLog AS CPQ WITH (NOLOCK), vwMMV AS VM WITH (NOLOCK)
						WHERE CBD.Id = CPQ.CBDId AND CBD.VersionId = VM.VersionId
						AND CPQ.IsPQRequested = 1 AND CBD.LeadId = @LeadId
						
						IF @PQReqBody > 0
							BEGIN
								--Record Available
								SET @SubCategoryId = 18
								
								--Get the estimate value
								SELECT @EstimateVal = Value
								FROM CRM.LSSubCategory WITH (NOLOCK) WHERE SubCategoryId = @SubCategoryId AND IsActive = 1
								
								IF @@ROWCOUNT > 0
									BEGIN								
										--Do other Operation
										SET @CategoryLeadScore =  (@PQReqBody * @EstimateVal)
										EXEC CRM.LSCalculateLeadScore @LeadId, @LSCategoryId, @SubCategoryId, @CategoryLeadScore
									END
							END
					--PQ Not Required
						SET @LSCategoryId = 11
						SET @CategoryLeadScore = NULL
						DECLARE @PQNR INT
						SELECT @PQNR = COUNT(DISTINCT VM.MakeId)   
						FROM CRM_CarBasicData CBD WITH (NOLOCK), CRM_CarPQLog AS CPQ WITH (NOLOCK), vwMMV AS VM WITH (NOLOCK)
						WHERE CBD.Id = CPQ.CBDId AND CBD.VersionId = VM.VersionId
						AND CPQ.IsPQNotRequired = 1 AND CBD.LeadId = @LeadId
						
						IF @PQNR > 0
							BEGIN
								--Record Available
								SET @SubCategoryId = 19
								
								--Get the estimate value
								SELECT @EstimateVal = Value
								FROM CRM.LSSubCategory WITH (NOLOCK) WHERE SubCategoryId = @SubCategoryId AND IsActive = 1
								
								IF @@ROWCOUNT > 0
									BEGIN								
										--Do other Operation
										SET @CategoryLeadScore =  (@PQNR * @EstimateVal)
										EXEC CRM.LSCalculateLeadScore @LeadId, @LSCategoryId, @SubCategoryId, @CategoryLeadScore
									END
							END
					END	
				
			END
			
		ELSE IF @SectionId = 5 -- Score Lead When there is a TD/TD Completed/TD Not Required
			
			BEGIN
				IF @LeadId <> -1
					BEGIN
					--TD Completed
						SET @LSCategoryId = 12
						DECLARE @TDNR INT
						DECLARE @TDCOMP INT
						SELECT @TDCOMP = SUM(CASE ISNULL(CTD.IsTDCompleted,0) WHEN 0 THEN 0 ELSE 1 END),  
						@TDNR = SUM(CASE ISNULL(CTD.ISTDNotPossible,0) WHEN 0 THEN 0 ELSE 1 END)     
						FROM CRM_CarBasicData CBD WITH (NOLOCK), CRM_CarTDLog AS CTD WITH (NOLOCK)
						WHERE CBD.Id = CTD.CBDId AND CBD.LeadId = @LeadId
						
						IF @TDCOMP > 0
							BEGIN
								--Record Available
								SET @SubCategoryId = 20
								
								--Get the estimate value
								SELECT @EstimateVal = Value
								FROM CRM.LSSubCategory WITH (NOLOCK) WHERE SubCategoryId = @SubCategoryId AND IsActive = 1
								
								IF @@ROWCOUNT > 0
									BEGIN								
										--Do other Operation
										SET @CategoryLeadScore =  (@TDCOMP * @EstimateVal)
										EXEC CRM.LSCalculateLeadScore @LeadId, @LSCategoryId, @SubCategoryId, @CategoryLeadScore
									END
							END
						
					--TD Not Possible
						SET @LSCategoryId = 13
						SET @LSCategoryId = 8
						SET @CategoryLeadScore = NULL
						IF @TDNR > 0
							BEGIN
								--Record Available
								SET @SubCategoryId = 21
								
								--Get the estimate value
								SELECT @EstimateVal = Value
								FROM CRM.LSSubCategory WITH (NOLOCK) WHERE SubCategoryId = @SubCategoryId AND IsActive = 1
								
								IF @@ROWCOUNT > 0
									BEGIN								
										--Do other Operation
										SET @CategoryLeadScore =  (@TDNR * @EstimateVal)
										EXEC CRM.LSCalculateLeadScore @LeadId, @LSCategoryId, @SubCategoryId, @CategoryLeadScore
									END
							END
					--TD Complete Delay
						SET @LSCategoryId = 14
						SET @CategoryLeadScore = NULL
						DECLARE @TDCOMPDelay INT
						SELECT @TDCOMPDelay = MAX(DATEDIFF(DD,TDRequestDate, TDCompleteDate))
						FROM CRM_CarBasicData CBD WITH (NOLOCK), CRM_CarTDLog AS CTD WITH (NOLOCK)
						WHERE CBD.Id = CTD.CBDId AND CTD.IsTDCompleted = 1 AND CBD.LeadId = @LeadId
						
						IF @TDCOMPDelay > 0
							BEGIN
								--Record Available
								SET @SubCategoryId = 22
								
								--Get the estimate value
								SELECT @EstimateVal = Value
								FROM CRM.LSSubCategory WITH (NOLOCK) WHERE SubCategoryId = @SubCategoryId AND IsActive = 1
								
								IF @@ROWCOUNT > 0
									BEGIN								
										--Do other Operation
										SET @CategoryLeadScore =  (@TDCOMPDelay * @EstimateVal)
										EXEC CRM.LSCalculateLeadScore @LeadId, @LSCategoryId, @SubCategoryId, @CategoryLeadScore
									END
							END
					END	
			END
			
		ELSE IF @SectionId = 6 -- Score Lead When there is a PE/PE Completed/PE Not Required
			
			BEGIN
				IF @LeadId <> -1
					BEGIN
					--PE Completed
						SET @LSCategoryId = 7
						DECLARE @PENR INT
						DECLARE @PECOMP INT
						SELECT @PECOMP = SUM(CASE ISNULL(CPE.IsPECompleted,0) WHEN 0 THEN 0 ELSE 1 END),  
						@PENR = SUM(CASE ISNULL(CPE.IsPENotRequired,0) WHEN 0 THEN 0 ELSE 1 END)     
						FROM CRM_CarBasicData CBD WITH (NOLOCK), CRM_CarPELog AS CPE WITH (NOLOCK)
						WHERE CBD.Id = CPE.CBDId AND CBD.LeadId = @LeadId
						
						IF @PECOMP > 0
							BEGIN
								--Record Available
								SET @SubCategoryId = 15
								
								--Get the estimate value
								SELECT @EstimateVal = Value
								FROM CRM.LSSubCategory WITH (NOLOCK) WHERE SubCategoryId = @SubCategoryId AND IsActive = 1
								
								IF @@ROWCOUNT > 0
									BEGIN								
										--Do other Operation
										SET @CategoryLeadScore =  (@PECOMP * @EstimateVal)
										EXEC CRM.LSCalculateLeadScore @LeadId, @LSCategoryId, @SubCategoryId, @CategoryLeadScore
									END
							END
						
					--PE Not Required
						SET @LSCategoryId = 8
						SET @CategoryLeadScore = NULL
						IF @PENR > 0
							BEGIN
								--Record Available
								SET @SubCategoryId = 16
								
								--Get the estimate value
								SELECT @EstimateVal = Value
								FROM CRM.LSSubCategory WITH (NOLOCK) WHERE SubCategoryId = @SubCategoryId AND IsActive = 1
								
								IF @@ROWCOUNT > 0
									BEGIN								
										--Do other Operation
										SET @CategoryLeadScore =  (@PENR * @EstimateVal)
										EXEC CRM.LSCalculateLeadScore @LeadId, @LSCategoryId, @SubCategoryId, @CategoryLeadScore
									END
							END
						
					END	
			END
			
		ELSE IF @SectionId = 7 -- Score lead on the basis of customer city
			BEGIN
				
				IF @CustomerId <> -1
					BEGIN
					--Customer City
						SET @LSCategoryId = 15
						DECLARE @CityTier AS INT
						SELECT @CityTier = LCT.TierId 
						FROM CRM.LSCityTier AS LCT WITH (NOLOCK), CRM_Customers AS CC WITH (NOLOCK)
						WHERE CC.CityId = LCT.CityId AND CC.ID = @CustomerId
						
						IF @@ROWCOUNT > 0
							BEGIN
		
								--Record Available
								SET @SubCategoryId = NULL
								
								--Get the estimate value
								SELECT @EstimateVal = Value, @SubCategoryId = SubCategoryId 
								FROM CRM.LSSubCategory WITH (NOLOCK) WHERE TierId = @CityTier AND IsActive = 1
								
								IF @@ROWCOUNT > 0
									BEGIN								
										--Do other Operation
										SET @CategoryLeadScore =  @EstimateVal
										EXEC CRM.LSCalculateLeadScore @LeadId, @LSCategoryId, @SubCategoryId, @CategoryLeadScore
									END
							END
					END		
			END
			
	END
	
END


