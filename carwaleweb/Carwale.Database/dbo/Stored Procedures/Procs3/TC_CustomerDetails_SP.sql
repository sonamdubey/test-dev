IF EXISTS (
    SELECT * FROM sysobjects WHERE id = object_id(N'[dbo].[TC_CustomerDetails_SP]') 
    AND xtype IN (N'P')
)
    DROP PROCEDURE [dbo].[TC_CustomerDetails_SP]
GO

	-- Modified By:	Nilesh Utture on 23-01-2013 added UPDATE Query to update TC_BuyerInquiries TABLE
-- Modified By:	Surendra on 25/05/2012
-- Description:	Customer is checking on the basi of mobile no before it was email
-- Modified By:	Nilesh Utture on 13 Feb 2013 Removed part that marks Inquiries Lead as conveerted bcoz it will happen when stock gets sold
-- Modified By: Nilesh Utture on 11th March, 2013 Updated LastUpdatedDate in TC_Stock
-- =============================================
CREATE PROCEDURE [dbo].[TC_CustomerDetails_SP] 	
	@BranchId NUMERIC,
	@CustomerName VARCHAR(100),
	@Email VARCHAR(100),
	@Mobile VARCHAR(15),
	@Address VARCHAR(200),
	@City INT,
	@Pincode VARCHAR(6),
	@Dob DATE=NULL,
	@DelDate DATE=NULL,
	@StockId INT,
	@UserId INT,
	@IsFinanceRequire BIT,
	@IsInsuranceRequire BIT,
	@Status INT OUTPUT,
	@InsuranceExp Date
AS
BEGIN
	SET @Status=0-- default value.it return any error occured
	DECLARE @Stockprice DECIMAL
	DECLARE @Customer_Id BIGINT=NULL
	SELECT @Customer_Id=CustomerId FROM TC_CarBooking WHERE StockId=@StockId AND IsCanceled=0
	DECLARE @BuyerInqId BIGINT
	DECLARE @InquiriesLeadId BIGINT
	DECLARE @LeadId BIGINT
	
	IF(@Customer_Id IS NULL)-- TC_CarBooking Id
		BEGIN-- For Inserting new entry in TC_CarBooking
			SELECT @Stockprice= Price FROM TC_Stock WHERE Id=@StockId--retrive stock price
			DECLARE @CustomerId INT
			
			-- Check Customer is already registered with dealer
			SELECT @CustomerId=C.Id FROM TC_CustomerDetails C WHERE C.Mobile=@Mobile AND C.BranchId=@BranchId AND C.IsActive=1
			IF (@CustomerId IS NULL) -- Customer is not registered with dealer
				BEGIN
					INSERT INTO TC_CustomerDetails(CustomerName, Email, Mobile,Address, City, Pincode, Dob,BranchId)
					VALUES(@CustomerName,@Email,@Mobile,@Address,@City,@Pincode,@Dob,@BranchId)	
					SET @CustomerId = SCOPE_IDENTITY()
					-- Book this car for new customer
					-- Check whether car is booked to some other customer
					IF NOT EXISTS(SELECT id FROM TC_Stock as s WHERE  s.Id=@StockId AND S.IsBooked=1 )
					BEGIN
						INSERT INTO TC_CarBooking(CustomerId,TotalAmount,StockId,Discount,NetPayment,UserId,DeliveryDate,BookingDate,IsFinanceRequire,IsInsuranceRequire) VALUES(@CustomerId,@Stockprice, @StockId,0,@Stockprice,@UserId,@DelDate,GETDATE(),@IsFinanceRequire,@IsInsuranceRequire)
						UPDATE TC_Stock SET IsBooked=1, LastUpdatedDate = GETDATE() WHERE Id=@StockId--change flag to this stock booked
						
						-- Modified By : Nilesh Utture on 14th February 2013
						SELECT @BuyerInqId= TC_BuyerInquiriesId, @LeadId = L.TC_LeadId, @InquiriesLeadId = B.TC_InquiriesLeadId 
						FROM  TC_BuyerInquiries B INNER JOIN TC_InquiriesLead L ON L.TC_InquiriesLeadId=B.TC_InquiriesLeadId
						WHERE B.StockId=@StockId AND L.TC_CustomerId=@CustomerId AND L.TC_LeadStageId<>3 AND L.IsActive = 1
						
						--SELECT @LeadId = TC_LeadId, @InquiriesLeadId = TC_InquiriesLeadId FROM TC_InquiriesLead WHERE TC_CustomerId = @CustomerId AND TC_LeadInquiryTypeId = 1 AND IsActive = 1
						
						UPDATE TC_BuyerInquiries  SET BookingStatus=34, BookingDate = GETDATE()
						WHERE TC_BuyerInquiriesId = @BuyerInqId -- Modified By:	Nilesh Utture on 23-01-2013
						EXEC TC_DispositionLogInsert @UserId,34,@BuyerInqId,3,@LeadId
			
					END
					SET @Status=1-- successfully Inserted
				END
			ELSE -- Customer is already exist so retrieving customer id and car is booked for that customer
				BEGIN
				-- Modified By:	Nilesh Utture on 23-01-2013 Even if customer is exisiting need to update his details 
					UPDATE TC_CustomerDetails SET CustomerName=@CustomerName, Email=@Email, Mobile=@Mobile,Address=@Address,
					City=@City,Pincode=@Pincode,Dob=@Dob,ModifiedDate=GETDATE(),ModifiedBy=@UserId
					WHERE Id=@CustomerId AND BranchId=@BranchId
					
					UPDATE TC_TaskLists SET CustomerName=@CustomerName, CustomerEmail=@Email, CustomerMobile=@Mobile
					WHERE CustomerId=@CustomerId AND BranchId=@BranchId

					INSERT INTO TC_CarBooking(CustomerId,TotalAmount,StockId,Discount,NetPayment,UserId,DeliveryDate,BookingDate,IsFinanceRequire,IsInsuranceRequire) VALUES(@CustomerId,@Stockprice, @StockId,0,@Stockprice,@UserId,@DelDate,GETDATE(),@IsFinanceRequire,@IsInsuranceRequire)
					UPDATE TC_Stock SET IsBooked=1, LastUpdatedDate = GETDATE() WHERE Id=@StockId--change flag to this stock booked
					
					SELECT @BuyerInqId= TC_BuyerInquiriesId, @LeadId = L.TC_LeadId, @InquiriesLeadId = B.TC_InquiriesLeadId 
					FROM  TC_BuyerInquiries B WITH (NOLOCK) INNER JOIN TC_InquiriesLead L WITH (NOLOCK) ON L.TC_InquiriesLeadId=B.TC_InquiriesLeadId
					WHERE B.StockId=@StockId AND L.TC_CustomerId=@CustomerId AND L.TC_LeadStageId<>3 AND L.IsActive = 1
					
					--SELECT  FROM TC_InquiriesLead WITH (NOLOCK) WHERE TC_CustomerId = @CustomerId AND TC_LeadInquiryTypeId = 1 AND IsActive = 1
						
					UPDATE TC_BuyerInquiries  SET BookingStatus=34, BookingDate = GETDATE()
					WHERE TC_BuyerInquiriesId = @BuyerInqId -- Modified By:	Nilesh Utture on 23-01-2013
					EXEC TC_DispositionLogInsert @UserId,34,@BuyerInqId,3,@LeadId
		
					SET @Status=2 	
				END
				IF(@InsuranceExp IS NOT NULL)
				BEGIN
					UPDATE TC_CarCondition SET InsuranceExpiry=@InsuranceExp WHERE StockId=@StockId
				END 
		END
	ELSE
		BEGIN--For udating customer details
			IF NOT EXISTS(Select C.Id FROM TC_CustomerDetails C WITH (NOLOCK) WHERE C.Id<>@Customer_Id AND C.Mobile=@Mobile AND C.BranchId=@BranchId AND C.IsActive=1)
			BEGIN
				UPDATE TC_CustomerDetails SET CustomerName=@CustomerName, Email=@Email, Mobile=@Mobile,Address=@Address,
				City=@City,Pincode=@Pincode,Dob=@Dob,ModifiedDate=GETDATE(),ModifiedBy=@UserId
				WHERE Id=@Customer_Id AND BranchId=@BranchId

				UPDATE TC_TaskLists SET CustomerName=@CustomerName, CustomerEmail=@Email, CustomerMobile=@Mobile
				WHERE CustomerId=@CustomerId AND BranchId=@BranchId
			END
			
			UPDATE TC_CarBooking SET DeliveryDate=@DelDate ,IsFinanceRequire=@IsFinanceRequire,
			IsInsuranceRequire=@IsInsuranceRequire,ModifiedDate=GETDATE(),ModifiedBy=@UserId
			WHERE StockId=@StockId AND CustomerId=@Customer_Id AND IsCanceled = 0
			
			SET @Status=3 -- successfully updated
			
			IF(@InsuranceExp IS NOT NULL)
			BEGIN
				UPDATE TC_CarCondition SET InsuranceExpiry=@InsuranceExp WHERE StockId=@StockId
			END
			
		END
END
