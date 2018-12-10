IF EXISTS (
    SELECT * FROM sysobjects WHERE id = object_id(N'[dbo].[TC_CaptureInvoiceDate]') 
    AND xtype IN (N'P')
)
    DROP PROCEDURE [dbo].[TC_CaptureInvoiceDate]
GO

	-- =============================================
-- Author:		Vivek Gupta
-- Create date: 02-09-2013
-- Description:	Updating Retail(invoice) details for new car booked inquiries.
-- Modified By ; Tejashree Patil on 7 Nov 2013, Updated VW Offers and Payment Mode.
-- Modified By ; Tejashree Patil on 16 Dec 2013, Checked whether dealer inventory related sharing/mapping is done or not.
-- Modified By ; Tejashree Patil on 26 Dec 2013, Checked whether dealer inventory related sharing/mapping is done or not for VW.
-- Modified By ; Tejashree Patil on 2 Jan 2014, Reverted to SP date 7 Nov 2013.
-- Modified By : Manish Chourasiya on 09-01-2013 for updateing column RetailStatus in Tc_NewCarBooking Table in case of Retails value should by 81
-- Modified By : Vishal Srivastava on 31-01-2014 1430 HRS IST checking whether @ChassisNumber is available or not.
-- Modified By : Tejashree Patil on 5 Jan 2014, Commented code of checking @ChassisNumber is available or not.
-- =============================================
CREATE PROCEDURE [dbo].[TC_CaptureInvoiceDate]
@InqId BIGINT,
@InvoiceDate DATE,
@UserId BIGINT,
@TC_LeadId BIGINT,
@Salutation VARCHAR(15),
@FirstName VARCHAR(50),
@LastName VARCHAR(50),
@Mobile VARCHAR(20),
@ChassisNumber VARCHAR(50),
@Status TINYINT = NULL OUTPUT,
@TC_PaymentModeId INT=NULL,
@TC_OffersId INT=NULL
AS
BEGIN
    SET NOCOUNT ON;
   
    SET @Status = 0
   
    IF @InqId IS NOT NULL
    BEGIN
	-- Modified By : Vishal Srivastava on 31-01-2014 1430 HRS IST checking whether @ChassisNumber is available or not.
			 /*IF EXISTS(		 
							 SELECT DM.MakeId
							 FROM	TC_Users U WITH(NOLOCK)
									INNER JOIN	TC_DealerMakes DM WITH(NOLOCK)  ON DM.DealerId=U.BranchId
									INNER JOIN  Dealers D WITH(NOLOCK) ON D.Id = U.BranchId
							 WHERE	DM.MakeId=20 
									AND U.Id=@UserId
									AND D.DealerCode IS NOT NULL
					   )
			 BEGIN
				 IF NOT EXISTS(	SELECT  ChassisNumber 
								FROM    TC_StockInventory WITH(NOLOCK)
								WHERE   ChassisNumber=@ChassisNumber)
				   BEGIN
					   SET @Status = 5    --Means mapping is canceled and at same time retial is done, means mismatch data.
					   RETURN
				   END
			 END*/-- Modified By ; Tejashree Patil on 5 Jan 2014, Commented code of checking @ChassisNumber is available or not.
			 
             IF NOT EXISTS(SELECT TOP(1) TC_NewCarBookingId FROM TC_NewCarBooking WHERE ChassisNumber = @ChassisNumber)
                BEGIN
                   
                    SET @Status = 1 -- Set status as ChassisNo( Complete VIN No. is not duplicate)
                   
                    UPDATE TC_NewCarBooking
                    SET InvoiceDate = @InvoiceDate,
                        LastUpdatedDate = GETDATE() ,
                        BookingName = @FirstName,
                        BookingMobile = @Mobile,
                        Salutation = @Salutation,
                        LastName = @LastName,
                        ChassisNumber = @ChassisNumber,
                        TC_PaymentModeId =@TC_PaymentModeId,  ----Added by Tejashree Patil on 7 Nov 2013, Updated VW Offers and Payment Mode.
                        TC_OffersId =@TC_OffersId,
						RetailStatus=81                      -- Added By : Manish Chourasiya on 09-01-2013 for updateing column RetailStatus
                    WHERE TC_NewCarInquiriesId = @InqId
                    AND BookingStatus = 32
                   
                   
                    EXEC TC_DispositionLogInsert @UserId,81,@InqId,5,@TC_LeadId
                END
          ELSE IF  EXISTS(SELECT TOP(1) TC_NewCarBookingId FROM TC_NewCarBooking WHERE ChassisNumber = @ChassisNumber AND TC_NewCarInquiriesId=@InqId )
                BEGIN
                  SET @Status = 1 -- Set status as ChassisNo( Complete VIN No. is not duplicate)
                    UPDATE TC_NewCarBooking
                    SET InvoiceDate = @InvoiceDate, LastUpdatedDate = GETDATE() ,
                        BookingName = @FirstName, BookingMobile = @Mobile,
                        Salutation = @Salutation, LastName = @LastName,                       
                        TC_PaymentModeId =@TC_PaymentModeId,  ----Added by Tejashree Patil on 7 Nov 2013, Updated VW Offers and Payment Mode.
                        TC_OffersId =@TC_OffersId,
						RetailStatus=81                      -- Added By : Manish Chourasiya on 09-01-2013 for updateing column RetailStatus
                    WHERE TC_NewCarInquiriesId = @InqId AND BookingStatus = 32
                    
                    EXEC TC_DispositionLogInsert @UserId,81,@InqId,5,@TC_LeadId
                END
    END
END



