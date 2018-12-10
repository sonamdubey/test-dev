IF EXISTS (
    SELECT * FROM sysobjects WHERE id = object_id(N'[dbo].[AbSure_SaveVerificationDetails]') 
    AND xtype IN (N'P')
)
    DROP PROCEDURE [dbo].[AbSure_SaveVerificationDetails]
GO

	
-- =============================================
-- Author:	Nilima More On 21st October 2015
-- Description : To save warranty verification Details.	
-- =============================================
CREATE PROCEDURE [dbo].[AbSure_SaveVerificationDetails] @CarId INT
	,@CustName VARCHAR(150)
	,@Address VARCHAR(500)
	,@Mobile VARCHAR(20)
	,@AlternatePhone VARCHAR(20)
	,@Email VARCHAR(50)
	,@Model VARCHAR(100)
	,@MakeYear DATETIME
	,@RegNumber VARCHAR(50)
	,@RegistrationDate DATETIME
	,@Kilometer INT
	,@WarrantyStartDate DATETIME
	,@WarrantyEndDate DATETIME
	,@EngineNo VARCHAR(50)
	,@ChassisNo VARCHAR(50)
AS
BEGIN
	DECLARE @OrigCarId INT=NULL, @Addr VARCHAR(500) = NULL
	SELECT @Addr = REPLACE(@Address,CHAR(13)+CHAR(10),' ')
	SELECT @Addr = REPLACE(REPLACE(REPLACE(@Addr,' ','<>'),'><',''),'<>',' ')
	SELECT @Addr = REPLACE(@Addr, char(9), '') 

	SELECT @OrigCarId = dbo.Absure_GetMasterCarId(@RegNumber,@CarId)
	IF EXISTS (
			SELECT AbSure_CarDetailsId
			FROM AbSure_ActivatedWarrantyLog WITH (NOLOCK)
			WHERE AbSure_CarDetailsId = @OrigCarId
				AND IsActive = 1
			)
		UPDATE AbSure_ActivatedWarrantyLog
		SET IsActive = 0
		WHERE AbSure_CarDetailsId = @OrigCarId
		 
		
		 
	INSERT INTO AbSure_ActivatedWarrantyLog (
		AbSure_CarDetailsId
		,CustName
		,Address
		,Mobile
		,AlternatePhone
		,Email
		,EngineNo
		,ChassisNo
		,IsActive
		)
	VALUES (
		@OrigCarId
		,@CustName
		,@Addr
		,@Mobile
		,@AlternatePhone
		,@Email
		,@EngineNo
		,@ChassisNo
		,1
		)

	UPDATE AbSure_WarrantyActivationPending
	SET CustName = @CustName
		,Address = @Addr
		,Mobile = @Mobile
		,AlternatePhone = @AlternatePhone
		,Email = @Email
		,EngineNo = @EngineNo
		,ChassisNo = @ChassisNo
	WHERE AbSure_CarDetailsId = @OrigCarId
		AND IsActive = 1 
END