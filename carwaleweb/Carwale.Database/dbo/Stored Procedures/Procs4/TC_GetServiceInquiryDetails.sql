IF EXISTS (
    SELECT * FROM sysobjects WHERE id = object_id(N'[dbo].[TC_GetServiceInquiryDetails]') 
    AND xtype IN (N'P')
)
    DROP PROCEDURE [dbo].[TC_GetServiceInquiryDetails]
GO

	-- =============================================
-- Author:		Vicky Gupta
-- Create date: 15th Oct 2015
-- Description:	To get Service Inquiry Details of a given Service Inquiry Id
-- Modified By Vivek Gupta, 05-11-2015, handled versionid null condition
-- =============================================
CREATE PROCEDURE [dbo].[TC_GetServiceInquiryDetails] @ServiceInquiryId INT
AS
BEGIN
	SELECT TOP 1 SI.*
		,VW.MakeId
		,VW.ModelId
		,VW.Car
	FROM TC_ServiceInquiries SI WITH (NOLOCK)
	LEFT JOIN vwMMV AS VW WITH (NOLOCK) ON (
			SI.VersionId = VW.VersionId
			OR SI.CarModelId = VW.ModelId
			)
	WHERE SI.TC_ServiceInquiriesId = @ServiceInquiryId
END
