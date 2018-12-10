IF EXISTS (
    SELECT * FROM sysobjects WHERE id = object_id(N'[dbo].[TC_FeedbackCalling_InqDealerCarDetails_V16]') 
    AND xtype IN (N'P')
)
    DROP PROCEDURE [dbo].[TC_FeedbackCalling_InqDealerCarDetails_V16]
GO

	--===============================================================================
-- Author:     Tejashree Patil on 13 Oct 2016
-- Discription: To get selected dealer details and city area and inq car details.
-- EXEC [TC_FeedbackCalling_InqDealerCarDetails_V16.10.1] 4,30657
--===============================================================================
CREATE PROCEDURE [dbo].[TC_FeedbackCalling_InqDealerCarDetails_V16.10.1]
(
 @DealerId INT= NULL,  
 @InqLeadId INT= NULL
)
AS
BEGIN

	IF (@InqLeadId IS NOT NULL) 
	BEGIN
		DECLARE @CarDetails VARCHAR(100) = NULL

		SELECT	TOP 1 @DealerId = FCI.OldDealerId, @CarDetails = VW.Car 
		FROM	TC_InquiriesLead L WITH(NOLOCK)
			INNER JOIN TC_FeedbackCalling_Inquiries FCI WITH(NOLOCK) ON FCI.TC_InquiriesLeadId = L.TC_InquiriesLeadId
			INNER JOIN vwAllMMV VW WITH(NOLOCK) ON VW.VersionId = FCI.VersionId
		WHERE	L.TC_InquiriesLeadId = @InqLeadId
	END

	SELECT	Organization, C.Name CityName, A.Name AreaName, @CarDetails CarDetails
	FROM	Dealers D WITH(NOLOCK)
			INNER JOIN Cities C WITH(NOLOCK) ON C.ID = D.CityId
			LEFT JOIN Areas A WITH(NOLOCK) ON A.Id = D.AreaId
	WHERE	D.id = @DealerId
END

