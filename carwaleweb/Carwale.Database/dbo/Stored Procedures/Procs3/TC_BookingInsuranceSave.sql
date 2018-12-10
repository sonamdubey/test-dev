IF EXISTS (
    SELECT * FROM sysobjects WHERE id = object_id(N'[dbo].[TC_BookingInsuranceSave]') 
    AND xtype IN (N'P')
)
    DROP PROCEDURE [dbo].[TC_BookingInsuranceSave]
GO

	-- =============================================
-- Author:		Binumon George	
-- Create date: 17 Oct 2011
-- Description:	save insurance details for booking
-- =============================================
CREATE PROCEDURE [dbo].[TC_BookingInsuranceSave]
(
@StockId INT,
@DealerId INT,
@TC_BookingInsurance_Id   INT =NULL,
@TC_InsuranceCompany_Id INT,
@InsuranceType CHAR(15),
@PolicyNumber VARCHAR(20),
@PolicyStartDate DATE,
@PolicyEndDate DATE,
@SumInsured INT,
@Premiun INT,
@Status INT OUTPUT
)	
AS
BEGIN

	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;
	SET @Status=0
	IF EXISTS(SELECT Id	FROM TC_Stock WHERE Id=@StockId AND BranchId=@DealerId)--checking valid stock id and dealerid
		BEGIN
			DECLARE @TC_CarBooking_Id INT
			SELECT @TC_CarBooking_Id=TC_CarBookingId FROM TC_Carbooking WHERE stockId=@StockId--taking bookingid as per stockid
			
			IF(@TC_BookingInsurance_Id IS NULL AND @TC_CarBooking_Id IS NOT NULL)--checking @TC_BookingInsurance_Id null or not.if null insert
				BEGIN
					INSERT INTO TC_BookingInsurance(TC_CarBooking_Id, TC_InsuranceCompany_Id, InsuranceType, PolicyNumber, PolicyStartDate, PolicyEndDate, SumInsured)
					VALUES(@TC_CarBooking_Id, @TC_InsuranceCompany_Id,@InsuranceType,@PolicyNumber,@PolicyStartDate,@PolicyEndDate,@SumInsured )
					SET @Status=SCOPE_IDENTITY()
					IF NOT EXISTS(SELECT TC_CarBooking_Id FROM TC_PaymentOtherCharges WHERE TC_CarBooking_Id=@TC_CarBooking_Id AND TC_PaymentVariables_Id=3 )
						--For insert new row in other charge payment
						BEGIN
							INSERT TC_PaymentOtherCharges(TC_CarBooking_Id,Amount,TC_PaymentVariables_Id)VALUES(@TC_CarBooking_Id,0,3)
						END	
				END				
				ELSE
				BEGIN --if @TC_BookingInsurance_Id not null update row
					IF(@TC_CarBooking_Id IS NOT NULL)--If booking id not null
						BEGIN
							UPDATE TC_BookingInsurance SET TC_CarBooking_Id=@TC_CarBooking_Id, TC_InsuranceCompany_Id=@TC_InsuranceCompany_Id, InsuranceType=@InsuranceType,
							PolicyNumber=@PolicyNumber, PolicyStartDate=@PolicyStartDate, PolicyEndDate=@PolicyEndDate, SumInsured=@SumInsured WHERE TC_BookingInsurance_Id=@TC_BookingInsurance_Id
							SET @Status=@TC_BookingInsurance_Id
							IF NOT EXISTS(SELECT TC_CarBooking_Id FROM TC_PaymentOtherCharges WHERE TC_CarBooking_Id=@TC_CarBooking_Id AND TC_PaymentVariables_Id=3 )
								BEGIN
									--For insert new row in other charge payment
									INSERT TC_PaymentOtherCharges(TC_CarBooking_Id,Amount,TC_PaymentVariables_Id)VALUES(@TC_CarBooking_Id,0,3)
								END		
						END
						ELSE
						BEGIN
							SET @Status=-1--error.because stock not booked
						END
				END
		END
END


/****** Object:  StoredProcedure [dbo].[TC_BookingFinanceView]    Script Date: 11/10/2011 17:51:05 ******/
SET ANSI_NULLS ON
