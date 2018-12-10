IF EXISTS (
    SELECT * FROM sysobjects WHERE id = object_id(N'[dbo].[Classified_ReportListing_SP]') 
    AND xtype IN (N'P')
)
    DROP PROCEDURE [dbo].[Classified_ReportListing_SP]
GO

	/*
Name: Classified_ReportListing_SP
Created By: Vikas C.
Created On: 26-Mar-2012
Description: This procedure inserts a user complaint made against a particular listing into the table Classified_ReportListing.
*/
CREATE Procedure [dbo].[Classified_ReportListing_SP]
@InquiryId Numeric,
@InquiryType SmallInt,
@ReasonId Int,
@Description VarChar(MAX),
@CustomerId Numeric = NULL,
@EmailId VarChar(50),
@ComplaintId Numeric Output
As 
	if not Exists(select * from Classified_ReportListing where InquiryId = @InquiryId and EmailId = @EmailId)
Begin
	Insert Into Classified_ReportListing (InquiryId, InquiryType, ReasonId, Description, EmailId) Values (@InquiryId, @InquiryType, @ReasonId, @Description, @EmailId)
	Set @ComplaintId = SCOPE_IDENTITY()
End