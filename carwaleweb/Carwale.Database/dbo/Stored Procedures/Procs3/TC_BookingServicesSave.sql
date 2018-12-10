IF EXISTS (
    SELECT * FROM sysobjects WHERE id = object_id(N'[dbo].[TC_BookingServicesSave]') 
    AND xtype IN (N'P')
)
    DROP PROCEDURE [dbo].[TC_BookingServicesSave]
GO

	
-- =============================================
-- Author:		Binumon George
-- Create date: 11th October 2011
-- Description:	This procedure is used to add update booking services
-- =============================================
CREATE PROCEDURE [dbo].[TC_BookingServicesSave]
(
@TC_BookingServices_Id INT =NULL,
@DealerId NUMERIC,
@ServiceName VARCHAR(50)
)
AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;
	
	IF(@TC_BookingServices_Id IS NULL) --Insering Dealer's services
	BEGIN
		IF NOT EXISTS(SELECT * FROM TC_BookingServices WHERE DealerId=@DealerId AND ServiceName=@ServiceName AND IsActive=1)
		BEGIN
			INSERT TC_BookingServices(ServiceName,DealerId) VALUES(@ServiceName,@DealerId)
		END
		ELSE
		BEGIN
			RETURN -2 -- Means Duplicate record is already exists in DB
		END		
	END
	ELSE --  Updating  services
	BEGIN
		IF NOT EXISTS(SELECT * FROM TC_BookingServices WHERE DealerId=@DealerId AND TC_BookingServices_Id<>@TC_BookingServices_Id AND ServiceName=@ServiceName AND IsActive=1)
		BEGIN
			UPDATE TC_BookingServices SET ServiceName=@ServiceName WHERE TC_BookingServices_Id=@TC_BookingServices_Id
		END
		ELSE
		BEGIN
			RETURN -2 -- Means Duplicate record is already exists in DB
		END		
	END 	
	SELECT TC_BookingServices_Id ,ServiceName FROM TC_BookingServices WHERE DealerId=@DealerId AND IsActive=1    
END



