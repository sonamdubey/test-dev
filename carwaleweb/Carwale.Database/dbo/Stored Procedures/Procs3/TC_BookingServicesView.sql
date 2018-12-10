IF EXISTS (
    SELECT * FROM sysobjects WHERE id = object_id(N'[dbo].[TC_BookingServicesView]') 
    AND xtype IN (N'P')
)
    DROP PROCEDURE [dbo].[TC_BookingServicesView]
GO

	
/****** Object:  StoredProcedure [dbo].[TC_DealerBankSave]    Script Date: 10/05/2011 15:32:56 ******/
-- =============================================
-- Author:		Binumon George
-- Create date: 11th October 2011
-- Description:	This procedure will be used to View All Booking warranties
-- =============================================
CREATE PROCEDURE [dbo].[TC_BookingServicesView]
(
@DealerId NUMERIC
)

AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;
	SELECT TC_BookingServices_Id ,ServiceName FROM TC_BookingServices WHERE DealerId=@DealerId AND IsActive=1	
    
END


