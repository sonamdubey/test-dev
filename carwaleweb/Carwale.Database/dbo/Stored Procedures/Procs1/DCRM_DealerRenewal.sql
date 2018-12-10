IF EXISTS (
    SELECT * FROM sysobjects WHERE id = object_id(N'[dbo].[DCRM_DealerRenewal]') 
    AND xtype IN (N'P')
)
    DROP PROCEDURE [dbo].[DCRM_DealerRenewal]
GO

	-- =============================================
-- Author:		Vaibhav K
-- Create date: 23-July-2012
-- Description:	Checks for the Dealer Renewal
--				If Status of Dealer is open, Schedule a new call else enter a new record in DCRM_SalesDealer
-- =============================================
CREATE PROCEDURE [dbo].[DCRM_DealerRenewal]
	-- Add the parameters for the stored procedure here
	@DealerId			NUMERIC,
	@UpdatedBy			INT,
	@NewCallId			NUMERIC OUTPUT
		
AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET @NewCallId = -1
	
	SET NOCOUNT ON;
	DECLARE @LeadSource		INT = 1,
			@PitchProduct	INT,
			@PitchDuration	INT,
			@ClosingAmt		NUMERIC,
			@PkgExpiryDate	DATETIME,
			@CallerId		INT,
			@PackageD		VARCHAR(100),
			@CurrentDate	DATETIME,
			@ServiceBOExec	INT,
			@ServiceFieldExec INT,
			@Status			VARCHAR(50),
			@ExistCallId	NUMERIC,
			@DealerType		INT

    -- SET Today Date
    SET @CurrentDate = GETDATE()
    
    --Check if caller(Sales BO) is assigned for specific dealer
	SELECT @CallerId = ISNULL(UserId, -1) 
	FROM DCRM_ADM_UserDealers DUD
	WHERE DUD.DealerId = @DealerId AND DUD.RoleId=2
	
	--Sales BO Exists
	IF	@CallerId > 0
		BEGIN
			--Check for status of Dealer whether it is open or not
			SELECT Id FROM DCRM_SalesDealer WHERE DealerId = @DealerId AND LeadStatus = 1
			IF @@ROWCOUNT = 0
				BEGIN
					--Fetch ServiceBOExecutive
					SELECT @ServiceBOExec = ISNULL(UserId,-1) 
					FROM DCRM_ADM_UserDealers DUD
					WHERE DUD.DealerId = @DealerId AND DUD.RoleId=4
					
					--Fetch ServiceFieldExecutive
					SELECT @ServiceFieldExec = ISNULL(UserId,-1) 
					FROM DCRM_ADM_UserDealers DUD
					WHERE DUD.DealerId = @DealerId AND DUD.RoleId=5
	
					--Retrieve the dealer's data
					SELECT @LeadSource = ISNULL(D.DealerSource, 1),@PkgExpiryDate = CC.ExpiryDate, 
						@PackageD = (SELECT TOP 1 CONVERT(VARCHAR,Cpr.PackageId) + ':' + CONVERT(VARCHAR,Cpr.ActualValidity/30) + ':' + CONVERT(VARCHAR,Cpr.ActualAmount) 
					FROM Packages Pkg, ConsumerPackageRequests Cpr
					WHERE Pkg.Id = Cpr.PackageId  AND ConsumerType = 1 AND Cpr.IsActive = 1 AND Cpr.IsApproved = 1 
						AND Cpr.ConsumerId = D.id  AND Pkg.InqPtCategoryId = CC.PackageType AND Pkg.IsStockBased = 1 Order By Cpr.ID Desc)
					FROM Dealers AS D, ConsumerCreditPoints CC
					WHERE D.ID = CC.ConsumerId AND CC.ConsumerType = 1
						AND D.ID = @DealerId
					
					--Get package details separated by colon(:)
					SET @PackageD = LTRIM(RTRIM(@PackageD))
					
					--PitchingProduct
					DECLARE @Pos INT
					SET @Pos = CHARINDEX(':', @PackageD)
					SET @PitchProduct = LTRIM(RTRIM(LEFT(@PackageD, @Pos - 1)))
					--ClosingAmount
					SET @PackageD = RIGHT(@PackageD, LEN(@PackageD) - @Pos)
					SET @Pos = CHARINDEX(':', @PackageD, 1)
					SET @PitchDuration = LTRIM(RTRIM(LEFT(@PackageD, @Pos - 1)))
					--PitchDuration
					SET @PackageD = RIGHT(@PackageD, LEN(@PackageD) - @Pos)
					SET @ClosingAmt = @PackageD
					
					--Set the Dealer type according to pacakge expiry date of dealer
					DECLARE @DayDiff	INT
					SET @DayDiff = DATEDIFF(DD,@PkgExpiryDate,@CurrentDate)
					SELECT @DealerType = 
						CASE 
							WHEN @DayDiff <= 0 THEN 3 -- Renewal Before Expiry Date
							WHEN @DayDiff < 30 THEN 5 -- Expiry Within 30 Days
							WHEN @DayDiff < 90 THEN 2 -- Expiry Days above 30Days but Below 90Days
							WHEN @DayDiff > 90 THEN 4 -- Expiry Aboove 90Days
							ELSE 1 -- New Dealer 
						END
					
					--Insert the details into table DCRM_SalesDealer
					INSERT INTO DCRM_SalesDealer 
						(DealerId,EntryDate,LeadSource,DealerType,ClosingProbability,LeadStatus,PitchingProduct,
						PitchDuration,ClosingAmount,UpdatedBy,UpdatedOn,BOExecutive,FieldExecutive,LostReason,IsSalesAppointment,PackageExpiryDate)
					VALUES (@DEALERID,@CurrentDate,@LeadSource,@DealerType,10,1,@PitchProduct,
						@PitchDuration,@ClosingAmt,@UpdatedBy,@CurrentDate,@ServiceBOExec,@ServiceFieldExec,-1,0,@PkgExpiryDate)
					
				END
				
				--Schedule Call for Sales BO
				EXEC DCRM_ScheduleNewCall @DealerId,@CallerId,@CurrentDate,@UpdatedBy,@CurrentDate,'Dealer Renewal Alert',NULL,3,@NewCallId OUTPUT, @ExistCallId OUTPUT
				SET @Status = 'Ticket successfully generated for Dealer'
				SET @NewCallId = @ExistCallId
		END
	ELSE
		BEGIN
			--Save Dealer as Orphan Dealer.
			INSERT INTO DCRM_OrphanDealers (DealerId,Type)
			VALUES (@DealerId,2)
			SET @Status = 'This dealer does not have Sales BO executive. Ticket Failed!!'
		END
END