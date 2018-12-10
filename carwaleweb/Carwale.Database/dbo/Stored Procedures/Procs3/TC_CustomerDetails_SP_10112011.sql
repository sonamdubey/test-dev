IF EXISTS (
    SELECT * FROM sysobjects WHERE id = object_id(N'[dbo].[TC_CustomerDetails_SP_10112011]') 
    AND xtype IN (N'P')
)
    DROP PROCEDURE [dbo].[TC_CustomerDetails_SP_10112011]
GO

	-- =============================================
-- Author:		Binumon George
-- Create date: 25-08-2011
-- Description:	Insert and update TC_CustomerDetails table
-- =============================================
CREATE PROCEDURE [dbo].[TC_CustomerDetails_SP_10112011] 
	-- Add the parameters for the stored procedure here
	@Id NUMERIC=NULL,
	@CustomerName VARCHAR(100),
	@Email VARCHAR(100),
	@Mobile VARCHAR(10),
	@Address VARCHAR(200),
	@City INT,
	@Pincode VARCHAR(6),
	@Dob DATE=NULL,
	@DelDate DATE=NULL,
	@StockId INT,
	@UserId INT,
	@Status INT OUTPUT 
AS
BEGIN
	SET @Status=0-- default value.it return any error occured
	DECLARE @Stockprice DECIMAL
	IF(@Id IS NULL)
		BEGIN-- For Inserting new customer
			SELECT @Stockprice= Price FROM TC_Stock WHERE Id=@StockId--retrive stock price
			DECLARE @CustomerId INT
			SELECT @CustomerId=Id FROM TC_CustomerDetails WHERE (Email=@Email or Mobile = @Mobile)
			IF (@CustomerId IS NULL)
				BEGIN
					INSERT INTO TC_CustomerDetails(CustomerName, Email, Mobile,Address, City, Pincode, Dob)
					VALUES(@CustomerName,@Email,@Mobile,@Address,@City,@Pincode,@Dob)	
					
					INSERT INTO TC_CarBooking(CustomerId,TotalAmount,StockId,Discount,NetPayment,UserId,DeliveryDate,BookingDate) VALUES(SCOPE_IDENTITY(),@Stockprice, @StockId,0,@Stockprice,@UserId,@DelDate,GETDATE())
					UPDATE TC_Stock SET IsBooked=1 WHERE Id=@StockId--change flag to this stock booked
					SET @Status=1-- successfully Inserted
				END
			ELSE -- Customer is already exist so retrieving customer id and car is booked for that customer
				BEGIN
					INSERT INTO TC_CarBooking(CustomerId,TotalAmount,StockId,Discount,NetPayment,UserId,DeliveryDate,BookingDate) VALUES(@CustomerId,@Stockprice, @StockId,0,@Stockprice,@UserId,@DelDate,GETDATE())
					UPDATE TC_Stock SET IsBooked=1 WHERE Id=@StockId--change flag to this stock booked
					SET @Status=2 	
				END
		END
	ELSE
		BEGIN--For udating customer details
			IF NOT EXISTS(Select Id FROM TC_CustomerDetails WHERE Id<>@Id AND (Email=@Email or Mobile = @Mobile))
			BEGIN
				UPDATE TC_CustomerDetails SET CustomerName=@CustomerName, Email=@Email, Mobile=@Mobile,Address=@Address,City=@City,Pincode=@Pincode,Dob=@Dob
				WHERE Id=@Id
			END
			UPDATE TC_CarBooking SET DeliveryDate=@DelDate WHERE StockId=@StockId AND CustomerId=@Id
			SET @Status=3 -- successfully updated
		END
END

