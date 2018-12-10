IF EXISTS (
    SELECT * FROM sysobjects WHERE id = object_id(N'[dbo].[TC_BookingWarrantiesSave]') 
    AND xtype IN (N'P')
)
    DROP PROCEDURE [dbo].[TC_BookingWarrantiesSave]
GO

	
-- =============================================
-- Author:		Binumon George
-- Create date: 11th October 2011
-- Description:	This procedure is used to add update warranties
-- =============================================
CREATE PROCEDURE [dbo].[TC_BookingWarrantiesSave]
(
@TC_BookingWarranties_Id INT =NULL,
@DealerId NUMERIC,
@WarrantyName VARCHAR(50)
)
AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;
	
	IF(@TC_BookingWarranties_Id IS NULL) --Insering Dealer's warranty
	BEGIN
		IF NOT EXISTS(SELECT * FROM TC_BookingWarranties WHERE DealerId=@DealerId AND WarrantyName=@WarrantyName AND IsActive=1)
		BEGIN
			INSERT TC_BookingWarranties(WarrantyName,DealerId) VALUES(@WarrantyName,@DealerId)
		END
		ELSE
		BEGIN
			RETURN -2 -- Means Duplicate record is already exists in DB
		END		
	END
	ELSE --  Updating  Warranty
	BEGIN
		IF NOT EXISTS(SELECT * FROM TC_BookingWarranties WHERE DealerId=@DealerId AND TC_BookingWarranties_Id<>@TC_BookingWarranties_Id AND WarrantyName=@WarrantyName AND IsActive=1)
		BEGIN
			UPDATE TC_BookingWarranties SET WarrantyName=@WarrantyName WHERE TC_BookingWarranties_Id=@TC_BookingWarranties_Id
		END
		ELSE
		BEGIN
			RETURN -2 -- Means Duplicate record is already exists in DB
		END		
	END 	
	SELECT TC_BookingWarranties_Id ,WarrantyName FROM TC_BookingWarranties WHERE DealerId=@DealerId AND IsActive=1    
END



