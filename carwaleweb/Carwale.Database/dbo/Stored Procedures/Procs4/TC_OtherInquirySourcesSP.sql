IF EXISTS (
    SELECT * FROM sysobjects WHERE id = object_id(N'[dbo].[TC_OtherInquirySourcesSP]') 
    AND xtype IN (N'P')
)
    DROP PROCEDURE [dbo].[TC_OtherInquirySourcesSP]
GO

	
CREATE PROCEDURE [dbo].[TC_OtherInquirySourcesSP] 
@InquiryType SMALLINT,
@InquiryId int,
@SourceName VARCHAR(50)
AS 
BEGIN
	INSERT INTO TC_OtherInquirySources(InquiryType, InquiryId, SourceName)
	VALUES(@InquiryType,@InquiryId,@SourceName)
END








SET QUOTED_IDENTIFIER ON
