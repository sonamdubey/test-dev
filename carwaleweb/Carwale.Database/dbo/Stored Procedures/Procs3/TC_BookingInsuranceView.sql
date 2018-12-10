IF EXISTS (
    SELECT * FROM sysobjects WHERE id = object_id(N'[dbo].[TC_BookingInsuranceView]') 
    AND xtype IN (N'P')
)
    DROP PROCEDURE [dbo].[TC_BookingInsuranceView]
GO

	-- =============================================
-- Author:		Binumon George	
-- Create date: 17 Oct 2011
-- Description:	Getting insurance details for booking
-- =============================================
CREATE PROCEDURE [dbo].[TC_BookingInsuranceView]
(
@StockId INT,
@DealerId INT
)	
AS
BEGIN

	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;
		
	SELECT TC_BookingInsurance_Id, INS.TC_CarBooking_Id, TC_InsuranceCompany_Id, InsuranceType, PolicyNumber,
	  PolicyStartDate , PolicyEndDate, SumInsured, OT.Amount AS InsuranceAmount
	FROM TC_BookingInsurance INS
	INNER JOIN TC_CarBooking B ON INS.TC_CarBooking_Id=B.TC_CarBookingId
	LEFT JOIN TC_PaymentOtherCharges OT ON B.TC_CarBookingId=OT.TC_CarBooking_Id
	WHERE b.StockId=@StockId AND INS.IsActive=1 AND B.IsCanceled=0 AND OT.TC_PaymentVariables_Id=3
	
	SELECT TC_InsuranceCompany_Id, CompanyName FROM TC_InsuranceCompany WHERE DealerId=@DealerId AND IsActive=1
   
END


/****** Object:  StoredProcedure [dbo].[TC_BookingInsuranceSave]    Script Date: 11/10/2011 17:51:07 ******/
SET ANSI_NULLS ON
