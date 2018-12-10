IF EXISTS (
    SELECT * FROM sysobjects WHERE id = object_id(N'[CRM].[GetLeadScoreParametersCC]') 
    AND xtype IN (N'P')
)
    DROP PROCEDURE [CRM].[GetLeadScoreParametersCC]
GO

	
-- =============================================
-- Author:		Deepak Tripathi
-- Create date: 03-Feb-2014
-- Description:	Fetches all the paramaters required for lead score calculation at CC level
-- =============================================
CREATE PROCEDURE [CRM].[GetLeadScoreParametersCC]
	-- Add the parameters for the stored procedure here
	@CBDId NUMERIC
AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;
	
    -- Insert statements for procedure here
      
		DECLARE @LeadId BIGINT
		DECLARE @Eagerness INT 
		DECLARE @PurchaseTime INT 
		DECLARE @IsPQRequested BIT 
		DECLARE @IsTDRequested BIT 
		DECLARE @IsTempPQRequested BIT 
		DECLARE @IsTempTDRequested BIT 
		DECLARE @IsCarCompare BIT
		
		DECLARE @NumberRecords INT
		DECLARE @RowCount SMALLINT
		
		DECLARE @TempData Table(RowID INT IDENTITY(1, 1), LeadId NUMERIC, CBDId NUMERIC, Eagerness INT, 
									PurchaseTime INT, NoOfBookings INT, IsPQRequested BIT, IsTDRequested BIT, IsCarCompare BIT)
		
		-- Get the entire data
		INSERT INTO @TempData
		SELECT DISTINCT CL.ID AS LeadId, CDA.CBDId, ISNULL(CII.ClosingProbability,0) AS Eagerness,
				ISNULL(DATEDIFF(dd, CII.CreatedOn, CII.ClosingDate),0) AS PurchaseTime, ISNULL(CII.BookingCount,0) AS NoOfBookings,
				ISNULL(CPQ.IsPQRequested, 0) AS IsPQRequested, ISNULL(CTD.IsTDRequested,0) AS IsTDRequested, ISNULL(CVL.IsCarCompare,0) AS IsCarCompare
		FROM CRM_CarDealerAssignment CDA WITH (NOLOCK)
			INNER JOIN CRM_CarBasicData CBD WITH (NOLOCK) ON CDA.CBDId = CBD.ID
			INNER JOIN CRM_Leads CL WITH (NOLOCK) ON CBD.LeadId = CL.ID
			INNER JOIN CRM_InterestedIn CII WITH (NOLOCK) ON CL.ID = CII.LeadId AND CII.ProductTypeId = 1
			LEFT JOIN CRM_CarPQLog CPQ WITH (NOLOCK) ON CDA.CBDId = CPQ.CBDId
			LEFT JOIN CRM_CarTDLog CTD WITH (NOLOCK) ON CDA.CBDId = CTD.CBDId
			LEFT JOIN CRM_VerificationOthersLog CVL WITH (NOLOCK) ON CL.ID = CVL.LeadId
		WHERE CL.ID IN(SELECT LeadId FROM CRM_CarBasicData WITH (NOLOCK) WHERE Id = @CBDId)
			
			
        SET @NumberRecords = @@ROWCOUNT
		SET @RowCount = 1
        
        -- Get Parameters Data
		WHILE @RowCount <= @NumberRecords
			BEGIN
				SELECT @LeadId = LeadId, @Eagerness = Eagerness, @PurchaseTime = PurchaseTime, 
						@IsCarCompare = IsCarCompare, @IsTempPQRequested = IsPQRequested, @IsTempTDRequested = IsTDRequested
				FROM @TempData
				WHERE RowID = @RowCount
				
				IF @IsTempPQRequested = 1
					SET @IsPQRequested = 1
					
				IF @IsTempTDRequested = 1
					SET @IsTDRequested = 1
					
				SET @RowCount = @RowCount + 1
			END
		
		-- Calculate Parameters
		DECLARE @Eagerness_prm INT 
		DECLARE @PurchaseTime_prm INT 
		DECLARE @IsPQRequested_prm TINYINT 
		DECLARE @IsTDRequested_prm TINYINT 
		DECLARE @IsCarCompare_prm TINYINT
		
		--num_eagerness: eagerness level clocked in CRM - min(eagerness) but not '-1' in 
		SET @Eagerness_prm = @Eagerness
		
		--PQ
		IF @IsPQRequested = 1
			SET @IsPQRequested_prm = 1
		ELSE
			SET @IsPQRequested_prm = 0
			
		--TD
		IF @IsTDRequested = 1
			SET @IsTDRequested_prm = 1
		ELSE
			SET @IsTDRequested_prm = 0
			
		--Compare
		IF @IsCarCompare = 1
			SET @IsCarCompare_prm = 1
		ELSE
			SET @IsCarCompare_prm = 0
			
		--purtime_rank: purchase time mentioned by the customer in FLC - min(purchasetime) but not '-1'
		IF @PurchaseTime <= 0
			SET @PurchaseTime_prm = 1
		ELSE IF @PurchaseTime <= 14
			SET @PurchaseTime_prm = 2
		ELSE IF @PurchaseTime > 14
			SET @PurchaseTime_prm = 3
			
		--Calculate the score
		--score=-0.75995 + 0.14532*num_eagerness -0.38679 *compare +0.95340*PQ + 0.34885*TD-1.04467 *purtime_rank 
		DECLARE @LeadScore FLOAT
		SET @LeadScore = -0.75995 + 0.14532*@Eagerness_prm - 0.38679*@IsCarCompare_prm + 0.95340*@IsPQRequested_prm + 0.34885*@IsTDRequested_prm - 1.04467*@PurchaseTime_prm 
			 
		DECLARE @Probability FLOAT
		SET @Probability = (EXP(@LeadScore)/(1+EXP(@LeadScore)))
		
		UPDATE CRM_Leads SET CCLeadScore = @Probability WHERE ID = @LeadId

END


