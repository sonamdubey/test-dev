IF EXISTS (
    SELECT * FROM sysobjects WHERE id = object_id(N'[dbo].[TC_CustomerDetailsUpdate]') 
    AND xtype IN (N'P')
)
    DROP PROCEDURE [dbo].[TC_CustomerDetailsUpdate]
GO

	
-- =============================================
-- Author:		<Author,Nilesh Utture>
-- Create date: <Create Date,08/1/2013>
-- Description:	<Description,Updates customer Details in inquiriesfollowup>
-- Modified By: Vivek gupta on 12th Aug,2013, Added @Address Parameter to update customer details.
-- Modified By: Tejashree Patil on 29th Aug,2013, Added @LastName,@Salutation parameters to update customer details.
-- Modified By Tejashree Patil on 2 Sept,2013 Allow to update Address previous it was Address = ISNULL(@Address,Address).
-- Modified By : Khushaboo Patil on August 19, 2016,optimized sp removed select * and modified to select id in exists statement.
-- =============================================
CREATE PROCEDURE [dbo].[TC_CustomerDetailsUpdate]
	-- Add the parameters for the stored procedure here
	@BranchId BIGINT,
	@CustomerId BIGINT,
	@CustomerName VARCHAR(100),
	@CustomerEmail VARCHAR(100),
	@CustomerMobile VARCHAR(10),
	@ModifiedBy INT,
	@Address VARCHAR(200) = NULL,
	@LastName VARCHAR(100) = NULL,
	@Salutation VARCHAR(15) = NULL
AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;

    -- Insert statements for procedure here
	IF EXISTS(SELECT Id FROM TC_CustomerDetails WITH (NOLOCK) WHERE Mobile=@CustomerMobile AND BranchId = @BranchId  AND Id<>@CustomerId AND IsActive=1)
		BEGIN
			RETURN -1
		END
	ELSE
		BEGIN
			UPDATE TC_CustomerDetails SET CustomerName = @CustomerName, Mobile = @CustomerMobile, Email = @CustomerEmail, LastName=@LastName, Salutation=@Salutation,
			ModifiedBy = @ModifiedBy, ModifiedDate = GETDATE(), Address = @Address -- Modified By Tejashree Patil on 2 Sept,2013
			WHERE Id = @CustomerId AND BranchId = @BranchId

			--Modified By : Khushaboo Patil on August 19, 2016,optimized sp.

			UPDATE TC_TaskLists SET CustomerName=@CustomerName, CustomerEmail=@CustomerEmail, CustomerMobile=@CustomerMobile
			WHERE CustomerId=@CustomerId AND BranchId=@BranchId
			RETURN 1
		END
	
END
