IF EXISTS (
    SELECT * FROM sysobjects WHERE id = object_id(N'[dbo].[TC_MM_SaveMappedNumber]') 
    AND xtype IN (N'P')
)
    DROP PROCEDURE [dbo].[TC_MM_SaveMappedNumber]
GO

	
-- =============================================
-- Author:		<Khushaboo Patil>
-- Create date: <10 oct 2016 >
-- Description:	<Save mapped Numbers>
-- =============================================
create PROCEDURE [dbo].[TC_MM_SaveMappedNumber]
@MaskingNumber VARCHAR(10),
@MappedNumber VARCHAR(100),
@DealerId INT ,
@InquirySourceId INT,
@EnteredBy INT
AS
BEGIN
	-- DELETE ALL MAPPED NUMBERS TO MASKING NUMBER IF EXISTS
	IF EXISTS(SELECT TC_MM_MappedNumberId FROM TC_MM_MappedNumbers WITH(NOLOCK) WHERE MaskingNumber = @MaskingNumber)
	BEGIN
		DELETE FROM TC_MM_MappedNumbers WHERE MaskingNumber = @MaskingNumber
	END

	-- INSERT NEW MAPPED NUMBER
	INSERT INTO TC_MM_MappedNumbers (MaskingNumber,MappedNumber,DealerId,TC_InquirySourceId,EntryDate,EnteredBy)  
	SELECT @MaskingNumber,ListMember,@DealerId,@InquirySourceId,GETDATE(),@EnteredBy 
	FROM fnSplitCSVValues(@MappedNumber)

	-- FIND 1ST MAPPED NUMBER
	DECLARE @PrimaryContactId INT 
	SELECT TOP 1 @PrimaryContactId = TC_MM_MappedNumberID FROM TC_MM_MappedNumbers WITH(NOLOCK) WHERE MaskingNumber = @MaskingNumber

	-- UPDATE 1ST MAPPED NUMBER WITH IsPrimaryContact = 1 
	UPDATE TC_MM_MappedNumbers SET IsPrimaryContact = 1 WHERE TC_MM_MappedNumberID = @PrimaryContactId

	-- KEEP LOG OF NEW MAPPED NUMBERS
	INSERT INTO TC_MM_MappedNumbersLog(MaskingNumber,MappedNumber,IsPrimaryContact,DealerId,TC_InquirySourceId,ModifiedOn,ModifiedBy)  
	SELECT MaskingNumber,MappedNumber,IsPrimaryContact,DealerId,TC_InquirySourceId,GETDATE(),EnteredBy 
	FROM TC_MM_MappedNumbers WITH(NOLOCK)
	WHERE MaskingNumber = @MaskingNumber 
	
END 

