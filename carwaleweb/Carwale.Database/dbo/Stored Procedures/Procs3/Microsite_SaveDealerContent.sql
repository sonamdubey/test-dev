IF EXISTS (
    SELECT * FROM sysobjects WHERE id = object_id(N'[dbo].[Microsite_SaveDealerContent]') 
    AND xtype IN (N'P')
)
    DROP PROCEDURE [dbo].[Microsite_SaveDealerContent]
GO

	-- =============================================
-- Author:		<Chetan Kane>
-- Create date: <22/2/2012>
-- Description:	<To save dealer content to the database>
-- Modified By: Tejashree Patil on 6 May 2013 at 3.30 pm, Inserted ContentSubCatagoryId
-- =============================================
CREATE PROCEDURE [dbo].[Microsite_SaveDealerContent]
	@DealerId int,
	@ContentCategoryId int,
	@Content varchar(max),
	@ContentTitle varchar (200),
	@ContentSubCategoryId SMALLINT = NULL -- Modified By: Tejashree Patil for AboutUs Category
AS
BEGIN
	
    -- Insert statements for procedure here
	SET NOCOUNT ON;
	If EXISTS(	SELECT	DealerId 
				FROM	Microsite_DealerContent 
				WHERE	DealerId=@DealerId AND ContentCatagoryId=@ContentCategoryId 
						AND ISNULL(ContentSubCatagoryId,0)=ISNULL(@ContentSubCategoryId,0) AND IsActive=1)
	BEGIN
		UPDATE	Microsite_DealerContent 
		SET		DealerContent=@Content,ContentTitle=@ContentTitle,EntryDate=GETDATE() 
		WHERE	DealerId=@DealerId AND ContentCatagoryId=@ContentCategoryId 
				--AND ( ContentSubCatagoryId=@ContentSubCategoryId OR ContentSubCatagoryId IS NULL)
				AND ISNULL(ContentSubCatagoryId,0)=ISNULL(@ContentSubCategoryId,0)
	END
	ELSE
	BEGIN
		INSERT INTO Microsite_DealerContent(DealerId,DealerContent,ContentCatagoryId,ContentTitle,EntryDate,ContentSubCatagoryId) 
		VALUES		(@DealerId,@Content,@ContentCategoryId,@ContentTitle,GETDATE(),@ContentSubCategoryId)
	END
	 
END
