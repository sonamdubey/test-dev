IF EXISTS (
    SELECT * FROM sysobjects WHERE id = object_id(N'[dbo].[TC_GetCustomerDetails_SP]') 
    AND xtype IN (N'P')
)
    DROP PROCEDURE [dbo].[TC_GetCustomerDetails_SP]
GO
	-- =============================================
-- Modified By:	Surendra Chouksey
-- Create date: 20-10-2011
-- Description:	3 new(Finance,Insurance and Insurance expiry) added in select query
-- EXEC TC_GetCustomerDetails_SP 3795, 2
-- Modified by: Nilesh Utture on 22nd january, 2013 Added parameter @CustId and IF(@CustId IS NOT NULL AND @CustomerId IS NULL) condition
-- =============================================
CREATE  Procedure [dbo].[TC_GetCustomerDetails_SP] 
	@StockId INT=NULL,
	@CustId BIGINT = NULL -- Modified by: Nilesh Utture on 22nd january, 2013
AS
BEGIN
	DECLARE @CustomerId NUMERIC = NULL -- Modified by: Nilesh Utture on 22nd january, 2013
	DECLARE @BookingId INT		
		-- get  cutomer details
		IF(@StockId IS NOT NULL)
			BEGIN			
				-- getting all cities
				SELECT ID, NAME From Cities
				
				-- This coded is added to display Insurance Expiry no matter car is booked or not
				SELECT CC.InsuranceExpiry FROM TC_Stock ST WITH (NOLOCK)
				LEFT OUTER JOIN TC_CarCondition CC WITH (NOLOCK)  ON ST.Id=CC.StockId
				WHERE Id=@StockId

				SELECT @CustomerId = CustomerId,@BookingId=TC_CarBookingId FROM TC_CarBooking WITH (NOLOCK) WHERE StockId=@StockId AND IsCanceled=0
				--PRINT @CustomerId
				-- If Customer has already booked the car then to pre fill the details  
				IF(@CustomerId IS NOT NULL)
					BEGIN
						--DECLARE @FinanceReq TINYINT
						--DECLARE @InsuranceReq TINYINT
						--SELECT @FinanceReq=COUNT(TC_BookingFinance_Id) FROM TC_BookingFinance WHERE TC_CarBooking_Id=@BookingId AND IsActive=1
						--SELECT @InsuranceReq=COUNT(TC_BookingInsurance_Id) FROM TC_BookingInsurance WHERE TC_CarBooking_Id=@BookingId AND IsActive=1
						
						SELECT CD.Id,CD.CustomerName, CD.Email, CD.Mobile, CD.Address,CD.Pincode, CD.Dob,CB.DeliveryDate, CT.ID AS City,
						CB.IsFinanceRequire AS Finance,CB.IsInsuranceRequire AS Insurance
						FROM TC_CarBooking	CB WITH (NOLOCK)
						INNER JOIN TC_CustomerDetails CD WITH (NOLOCK) ON CB.CustomerId=CD.Id						
						LEFT OUTER JOIN Cities CT WITH (NOLOCK) on CT.ID=CD.City 											
						WHERE CD.Id=@CustomerId AND CD.IsActive=1 AND CB.StockId=@StockId AND CB.IsCanceled=0
					END			
				-- If new customer comes for first time to book the car
				IF(@CustId IS NOT NULL AND @CustomerId IS NULL)-- Modified by: Nilesh Utture on 22nd january, 2013
					BEGIN		
					PRINT @CustId			
						SELECT CD.Id,CD.CustomerName, CD.Email, CD.Mobile, CD.Address,CD.Pincode, CD.Dob, CT.ID AS City
						FROM TC_CustomerDetails CD WITH (NOLOCK)				
						LEFT OUTER JOIN Cities CT WITH (NOLOCK) on CT.ID=CD.City 											
						WHERE CD.Id=@CustId AND CD.IsActive=1
					END				
			END			
END
