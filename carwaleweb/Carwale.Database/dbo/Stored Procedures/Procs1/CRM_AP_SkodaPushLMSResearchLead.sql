IF EXISTS (
    SELECT * FROM sysobjects WHERE id = object_id(N'[dbo].[CRM_AP_SkodaPushLMSResearchLead]') 
    AND xtype IN (N'P')
)
    DROP PROCEDURE [dbo].[CRM_AP_SkodaPushLMSResearchLead]
GO

	-- =============================================
-- Author:		Deepak Tripathi
-- Create date: 27th Aug 2013
-- Modified By Manish on 28-08-2013 adding with (nolock) in the table CRM_PrePushData
-- =============================================
CREATE PROCEDURE [dbo].[CRM_AP_SkodaPushLMSResearchLead]
	
	AS

	BEGIN
	    
       	
		WITH CTE AS(
			SELECT DISTINCT VM.MakeId, C.Name, C.email, C.Mobile, C.CityId,
			(SELECT TOP 1 Id FROM CarVersions WHERE CarModelId = VM.ModelId AND New = 1 ORDER BY Id ASC) AS VersionId,
			 VM.Model AS CarName, VM.ModelId, C.Id AS CustomerId, NPI.RequestDateTime,
			 ROW_NUMBER() OVER(PARTITION BY C.Mobile ORDER BY NPI.RequestDateTime ASC) AS RowNum
			FROM NewCarPurchaseInquiries NPI WITH (NOLOCK) 
				INNER JOIN Customers AS C WITH (NOLOCK) ON NPI.CustomerId = C.Id
				INNER JOIN vwMMV VM ON NPI.CarVersionId = VM.VersionId 	
			WHERE NPI.ForwardedLead = 0 AND VM.MakeId = 15 AND VM.ModelId <> 47
			AND DAY(NPI.RequestDateTime) = DAY(GETDATE()-1) AND MONTH(NPI.RequestDateTime) = MONTH(GETDATE()-1) AND YEAR(NPI.RequestDateTime) = YEAR(GETDATE()-1)
			AND C.Mobile NOT IN(SELECT DISTINCT Mobile FROM CRM_PrePushData WITH (NOLOCK)  WHERE Result = 'SUCCESS' AND StartDate >= GETDATE()-30)
			AND CONVERT(BIGINT,C.Mobile) > 0
		)
		INSERT INTO CRM_SkodaResearchLeads(MakeId, Name, Email, Mobile, CityId, VersionId, CarName, ModelId, CWCustomerId)
		SELECT TOP (110 +CONVERT(INT, (20+1)*RAND())) MakeId, Name, email, Mobile, CityId, VersionId, CarName, ModelId, CustomerId 
		FROM CTE WHERE RowNum = 1

	

		
	END

	



