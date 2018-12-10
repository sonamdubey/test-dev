IF EXISTS (
    SELECT * FROM sysobjects WHERE id = object_id(N'[dbo].[TC_GetDealerPackage]') 
    AND xtype IN (N'P')
)
    DROP PROCEDURE [dbo].[TC_GetDealerPackage]
GO
	-- Author:		Upendra Kumar
-- Create date: 22 Sep 2015
-- Description:	To get dealer subscribed packages
CREATE procedure [dbo].[TC_GetDealerPackage] 
@DealerId INT 
AS
BEGIN
 SELECT P.Name, CPR.ActualAmount,CPR.Id AS ID, CPR.ApprovalDate AS StartDate, 
		DATEADD(day,CPR.ActualValidity, CPR.ApprovalDate) AS ExpiryDate, CPR.ActualInquiryPoints AS InquiryPoint, P.Description
 FROM ConsumerPackageRequests AS CPR WITH (NOLOCK) 
	INNER JOIN Packages AS P WITH (NOLOCK) ON  CPR.PackageId = P.Id 
 WHERE CPR.ConsumerId = @DealerId AND CPR.ConsumerType = 1 
 ORDER BY StartDate DESC

END


