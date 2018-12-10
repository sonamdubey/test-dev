IF EXISTS (
    SELECT * FROM sysobjects WHERE id = object_id(N'[dbo].[TC_Customer]') 
    AND xtype IN (N'P')
)
    DROP PROCEDURE [dbo].[TC_Customer]
GO

	-- Modified By:	Binu
-- Create date: 16 jun 2012
-- Description:Added param Address
---============================
-- Modified By:	Surendra
-- Create date: 21 MAY 2012
-- Description:	Now mobile is unique for dealer
-- =============================================
-- Created By:		Avishkar
-- Create date: 5 Jan 2012
-- Description:	Add newly used customer details if not exists 
--declare @customerId int
--exec [dbo].[TC_Customer] 5 ,'a@g.com' , 'a' , 0988,null,null,null,null ,@customerId out
--select @customerId
-- =============================================
CREATE PROCEDURE [dbo].[TC_Customer]
@BranchId NUMERIC, 
@CustomerEmail VarChar(100), 
@CustomerName VarChar(100),          
@CustomerMobile VARCHAR(15),

----------aded new param
@Location VARCHAR(50),
@Buytime VARCHAR(20),
@Comments VARCHAR(500),
@CreatedBy BIGINT,---------------
@customerId int  OUT ,
@Address VARCHAR(150)=NULL
AS                
	Begin          
		-- Checking Customer is already exists or not for that dealer 
		SET @customerId = IsNull(( Select top 1 Id From TC_CustomerDetails Where  Mobile= @CustomerMobile AND BranchId=@BranchId AND IsActive=1), 0 )
		If @customerId = 0         
			BEGIN      
				Insert Into TC_CustomerDetails( CustomerName, Mobile, Email,BranchId,Location,Buytime,Comments,CreatedBy,Address )                     
				Values( @CustomerName, @CustomerMobile, @CustomerEmail,@BranchId,@Location,@Buytime,@Comments ,@CreatedBy,@Address)  
				Set @customerId = IDENT_CURRENT('TC_CustomerDetails') 
			END
		ELSE
			BEGIN
				UPDATE TC_CustomerDetails SET CustomerName=@CustomerName, Mobile=@CustomerMobile, Email=@CustomerEmail,
					Location=@Location,Buytime=@Buytime,Comments=@Comments,ModifiedBy=@CreatedBy,ModifiedDate=GETDATE(),Address=@Address
					WHERE Id=@customerId AND BranchId=@BranchId

				UPDATE TC_TaskLists Set CustomerName = @CustomerName,  CustomerMobile = @CustomerMobile,  CustomerEmail = @CustomerEmail
				Where  CustomerId=@customerId AND BranchId=@BranchId
			END
	End
