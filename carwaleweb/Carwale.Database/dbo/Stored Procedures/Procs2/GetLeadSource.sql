IF EXISTS (
    SELECT * FROM sysobjects WHERE id = object_id(N'[dbo].[GetLeadSource]') 
    AND xtype IN (N'P')
)
    DROP PROCEDURE [dbo].[GetLeadSource]
GO

	--=========================================================
-- Author:		Vicky Lund
-- Create date: 27/04/2016
-- EXEC GetLeadSource 74,1
--=========================================================
CREATE PROCEDURE [dbo].[GetLeadSource] @PlatformId INT
	,@AdType INT
AS
BEGIN
	SELECT LS.SourceType AS LeadClickSourceDesc
		,LS.LeadClickSourceId
		,LS.InquirySourceId
	FROM LeadSource LS WITH (NOLOCK)
	WHERE LS.PlatformId = @PlatformId
		AND LS.AdType = @AdType
END
