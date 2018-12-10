IF EXISTS (
    SELECT * FROM sysobjects WHERE id = object_id(N'[dbo].[DCRM_SaveMaskingNumbers]') 
    AND xtype IN (N'P')
)
    DROP PROCEDURE [dbo].[DCRM_SaveMaskingNumbers]
GO

	-- =============================================
-- Author:		Ruchira Patil
-- Create date: 26th June 2014
-- Description:	To insert only those masking numbers in MM_AvailableNumbers which are not there in MM_SellerMobileMaskingLog and MM_AvailableNumbers
-- Modified by : 1. Manish on 31-07-2014 comment code for checking log table entries
--				 2. Ruchira Patil on 4th Aug 2014 (checking the entry in MM_SellerMobileMasking table) 
-- Modifier   :  Mihir Chheda[12-08-2016]
-- Description:  save masking number against stateId and serviceProviderId 
--               also if number is already maaped to some dealer or added in AvailableNumbers then return state name or dealer id respectively
-- =============================================
CREATE PROCEDURE [dbo].[DCRM_SaveMaskingNumbers] 
	@CityId INT,
	@Mobile VARCHAR(MAX),
	@AvailableMobile VARCHAR(MAX) OUTPUT,
	@StateId INT = NULL,
	@ServiceProvidersId INT = NULL
AS
BEGIN
    DECLARE @StateName VARCHAR(50)=NULL --Mihir Chheda[12-08-2016]
	DECLARE @DealerId  INT=NULL -- Mihir Chheda[12-08-2016]

	DECLARE @TempTblMobile TABLE
	(
	 ID INT IDENTITY(1,1),
	 Mobile VARCHAR(20)
	)
	DECLARE @i INT,@MobileCounter INT,@TempMobile VARCHAR(20),@TempVar INT =0
	
	SET @AvailableMobile = ''

	INSERT INTO @TempTblMobile(Mobile) SELECT ListMember FROM fnSplitCSVToChar(@Mobile)
	SET @i = 1
	SELECT @MobileCounter = COUNT(ID) FROM @TempTblMobile
	
	WHILE @MobileCounter > 0 
	BEGIN
		SELECT @TempMobile = REPLACE(REPLACE(REPLACE(Mobile, ' ',''),CHAR(10), ''), CHAR(13), '')  FROM @TempTblMobile WHERE ID = @i
		
		SELECT @StateName=s.Name FROM MM_AvailableNumbers(NOLOCK) man --Mihir Chheda[12-08-2016]
		JOIN States(NOLOCK) s ON s.ID=man.StateId
		WHERE MaskingNumber = @TempMobile 
		IF @@ROWCOUNT > 0
		BEGIN
			SET @TempVar = 1
			SET @AvailableMobile = @AvailableMobile + @TempMobile +' - '+ @StateName + ','
		END

		SELECT @DealerId=ConsumerId FROM MM_SellerMobileMasking(NOLOCK) --Mihir Chheda[12-08-2016]
	    WHERE MaskingNumber = @TempMobile
		IF @@ROWCOUNT > 0
		BEGIN
			SET @TempVar = @TempVar + 1
			SET @AvailableMobile = @AvailableMobile + @TempMobile +' - DealerId: ' + CAST(@DealerId AS VARCHAR(15)) +','
		END
		IF @TempVar = 0
		BEGIN
			INSERT INTO MM_AvailableNumbers(MaskingNumber,CityId,StateId,ServiceProvider) VALUES (@TempMobile,@CityId,@StateId,@ServiceProvidersId)
		END

		SET @i = @i + 1
		SET @MobileCounter = @MobileCounter - 1
		SET @TempVar = 0
	END
	IF @AvailableMobile <> ''
		SET @AvailableMobile = SUBSTRING(@AvailableMobile,1,LEN(@AvailableMobile)-1)
END

