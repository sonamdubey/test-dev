IF EXISTS (
    SELECT * FROM sysobjects WHERE id = object_id(N'[dbo].[TC_CheckBookingOnStock]') 
    AND xtype IN (N'P')
)
    DROP PROCEDURE [dbo].[TC_CheckBookingOnStock]
GO

	-- =============================================
-- Author:		<Author,Nilesh Utture>
-- Create date: <Create Date,23/01/2013>
-- Description:	<Description,Check If stock is already booked by other customer>
-- =============================================
CREATE PROCEDURE [dbo].[TC_CheckBookingOnStock]
	-- Add the parameters for the stored procedure here
	@StockId BIGINT,
	@InqId BIGINT,
	@CustomerName VARCHAR(200) OUTPUT
AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;

    -- Insert statements for procedure here
	SELECT @CustomerName= C.CustomerName 
	FROM TC_BuyerInquiries B WITH (NOLOCK)
	INNER JOIN TC_InquiriesLead I WITH (NOLOCK) ON B.TC_InquiriesLeadId = I.TC_InquiriesLeadId
	INNER JOIN TC_CustomerDetails C WITH (NOLOCK) ON C.Id = I.TC_CustomerId 
	WHERE B.StockId=@StockId AND B.BookingStatus=34 AND B.TC_BuyerInquiriesId<>@InqId
	
END
