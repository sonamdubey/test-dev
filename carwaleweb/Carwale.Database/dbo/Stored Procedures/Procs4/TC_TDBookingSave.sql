IF EXISTS (
    SELECT * FROM sysobjects WHERE id = object_id(N'[dbo].[TC_TDBookingSave]') 
    AND xtype IN (N'P')
)
    DROP PROCEDURE [dbo].[TC_TDBookingSave]
GO

	-- =============================================
-- Author:  Tejashree Patil on 7 Dec 2012 at 11 am
-- Description: Return details report of scheduled testdrive request
 --DECLARE @Status TINYINT , @TC_TDCalendarId BIGINT
 --EXEC TC_TDBookingSave 'Varsha',5,'1524897656','asdertyu',1,'sion',22,1,NULL,1,'BMW 3 Series','2012-10-02',
 --'11:00:00','12:30:00',NULL,1,1,@Status OUTPUT,@TC_TDCalendarId OUTPUT
 --SELECT @Status, @TC_TDCalendarId 
-- =============================================
CREATE PROCEDURE [dbo].[TC_TDBookingSave]
	@CustName VARCHAR(100),
	@BranchId BIGINT,
	@Mobile VARCHAR(15),
	@Address VARCHAR(150), 
	@SourceId TINYINT,
	@Area VARCHAR(100),
	@AreaId BIGINT ,
	@UserId BIGINT,
	@Email VARCHAR(100),
	@TC_TDCarsId INT,
    @TDCarDetails VARCHAR(100),
    @TDDate DATE,
    @TDStartTime TIME,
    @TDEndTime TIME,
    @ModifiedBy BIGINT,
    @TDConsultant BIGINT,
    @TC_TDDriverId BIGINT,
    @Status TINYINT OUTPUT,
	@TC_TDCalendarId BIGINT OUTPUT
AS
BEGIN

	-- Save Customer Details
	DECLARE @CustId BIGINT
	
	EXEC TC_Customer @BranchId,@Email,@CustName,@Mobile,@Area,NULL,NULL,@UserId,@CustId OUTPUT,@Address
		
	-- Save Car, Time , Date ,Executive, Driver Details
	
	IF (@TC_TDDriverId =-1)
    BEGIN
		SET @TC_TDDriverId=NULL
    END
    	
	INSERT INTO TC_TDCalendar(BranchId, TC_CustomerId, AreaName, ArealId, TC_UsersId, TC_SourceId, TDDriverId, 
				EntryDate, TC_TDCarsId, TDCarDetails, TDDate, TDStartTime, TDEndTime, TDStatus, ModifiedDate )
	VALUES		(@BranchId,@CustId,@Area,@AreaId,@UserId,@SourceId, @TC_TDDriverId,
				GETDATE(),@TC_TDCarsId,@TDCarDetails, @TDDate, @TDStartTime, @TDEndTime, @Status, GETDATE())
	
	SET @TC_TDCalendarId=SCOPE_IDENTITY()
	
END
