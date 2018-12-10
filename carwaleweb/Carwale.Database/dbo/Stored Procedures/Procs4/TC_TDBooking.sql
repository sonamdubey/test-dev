IF EXISTS (
    SELECT * FROM sysobjects WHERE id = object_id(N'[dbo].[TC_TDBooking]') 
    AND xtype IN (N'P')
)
    DROP PROCEDURE [dbo].[TC_TDBooking]
GO

	
-- =============================================
-- Author:		Binumon George
-- Create date: 11-07-2012
-- Description:	Added email parameter
-- Author:		Binumon George
-- Create date: 20-06-2012
-- Description:	Inserting Td customer details
-- =============================================
CREATE PROCEDURE [dbo].[TC_TDBooking]
	-- Add the parameters for the stored procedure here
	@CustName VARCHAR(100),
	@BranchId BIGINT,
	@Mobile VARCHAR(15),
	@Address VARCHAR(150), 
	@SourceId TINYINT,
	@Area VARCHAR(100),
	@AreaId BIGINT ,
	@UserId BIGINT,
	@TC_TDCalendarId BIGINT OUTPUT,
	@Email VARCHAR(100)
AS
BEGIN
	DECLARE @CustId BIGINT
		BEGIN
			EXEC TC_Customer @BranchId,@Email,@CustName,@Mobile,@Area,NULL,NULL,@UserId,@CustId OUTPUT,@Address
			 
			INSERT INTO TC_TDCalendar(BranchId, TC_CustomerId, AreaName, ArealId, TC_UsersId, TC_SourceId)
			VALUES(@BranchId,@CustId,@Area,@AreaId,@UserId,@SourceId)
			SET @TC_TDCalendarId=SCOPE_IDENTITY()
		END
END


