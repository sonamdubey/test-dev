IF EXISTS (
    SELECT * FROM sysobjects WHERE id = object_id(N'[dbo].[TC_DownloadNSCExcelInquiries]') 
    AND xtype IN (N'P')
)
    DROP PROCEDURE [dbo].[TC_DownloadNSCExcelInquiries]
GO

	-- =============================================
-- Author:		Vishal Srivastava AE1830	
-- Create date: 17-01-2014 1500 HRS IST
-- Description:	Downoad Unassigned Excel Inquiries
-- [TC_UnassignedExcelInquiries] 9,NULL,'2014-01-01 16:23:52.467','2014-02-19 16:23:52.467'
-- [TC_UnassignedExcelInquiries] 9,NULL,'2014-01-01','2014-02-19'
-- =============================================
CREATE PROCEDURE [dbo].[TC_DownloadNSCExcelInquiries]
	-- Add the parameters for the stored procedure here
	@UserId INT,
	@BranchId INT,
	@FromDate DATETIME,
	@ToDate DATETIME
AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;

    -- Insert statements for procedure here
			SELECT
					E.DealerCode AS 'Dealer Code',
					E.Salutation AS 'Salutation',
					E.Name AS 'First Name',
					E.LastName AS 'Last Name',
					E.Email,
					E.Mobile,
					E.City,
					E.CarModel AS 'Car Model',
					E.CarVersion AS 'Car Version',
					E.InquirySource AS Source
			FROM	TC_ExcelInquiries E WITH(NOLOCK)
			WHERE	E.IsValid=1
					AND E.UserId=@UserId 
					AND (@BranchId IS NULL OR E.BranchId=@BranchId)
					AND ((@FromDate IS NULL AND @ToDate IS NULL) or CONVERT(DATE,E.EntryDate) BETWEEN @FromDate AND @ToDate)
				
END

