IF EXISTS (
    SELECT * FROM sysobjects WHERE id = object_id(N'[dbo].[TC_GetInqLeads_11APR]') 
    AND xtype IN (N'P')
)
    DROP PROCEDURE [dbo].[TC_GetInqLeads_11APR]
GO
	

-- Author:		Binumon George
-- Create date: 08th Feb 2011
-- Description:	For Listing assigned and  Un assigned leads in Excel sheet
-- =============================================
CREATE PROCEDURE [dbo].[TC_GetInqLeads_11APR]
@BranchId INT,
@StatusuId INT=NULL,
@MakeId INT=NULL,
@ModelId INT=NULL,
@Priority INT=NULL,
@UserId INT=NULL,
@SourceId INT=NULL,
@LeadType INT=NULL,
@AssignType INT,
@FromDate DATETIME=NULL,
@ToDate DATETIME=NULL
AS
BEGIN
SET NOCOUNT ON
 DECLARE @Sql VARCHAR(MAX)
	--SET @FromDate=GETDATE()-20
	--SET @ToDate=GETDATE()
	SET @Sql='SELECT * from vwTC_InquiriesForExcel WHERE BranchId ='
	
		SET @Sql=@Sql+ CAST(@BranchId AS VARCHAR(5))
		
		IF(@AssignType = 0)--checking here lead assigned or not
		BEGIN
			SET @Sql += ' AND TC_UserId IS NULL'
		END
		ELSE
		BEGIN
			SET @Sql += ' AND TC_UserId IS NOT NULL'
		END
		IF(@StatusuId IS NOT NULL)
			BEGIN
				SET @Sql=@Sql+' AND TC_InquiriesFollowupActionId=' + CAST(@StatusuId AS VARCHAR(5))
			END
		IF(@MakeId IS NOT NULL)
			BEGIN
				SET @Sql=@Sql+' AND MakeId =' + CAST(@MakeId AS VARCHAR(5))
			END
		IF(@ModelId IS NOT NULL)
			BEGIN
				SET @Sql=@Sql+' AND ModelId =' + CAST(@ModelId AS VARCHAR(5))
			END
		IF(@Priority IS NOT NULL)  
			BEGIN
				SET @Sql=@Sql+' AND TC_InquiryStatusId =' + CAST(@Priority AS VARCHAR(5))
			END
		IF(@UserId IS NOT NULL)
			BEGIN
				SET @Sql=@Sql+' AND TC_UserId =' + CAST(@UserId AS VARCHAR(5))
			END
		IF(@SourceId IS NOT NULL)
			BEGIN
				SET @Sql=@Sql+' AND SourceId =' + CAST(@SourceId AS VARCHAR(5))
			END
		IF(@LeadType IS NOT NULL)
			BEGIN
				SET @Sql=@Sql+' AND TC_InquiryTypeId =' + CAST(@LeadType AS VARCHAR(5))
			END
		IF(@FromDate IS NOT NULL AND @ToDate IS NOT NULL )
			BEGIN
				SET @Sql=@Sql+' AND  InquiryDate BETWEEN ''' + CAST(@FromDate AS VARCHAR(20)) + ''' AND ''' + CAST(@ToDate AS VARCHAR(20))+''''  
			END
		ELSE IF(@FromDate IS NOT NULL)
			BEGIN
				SET @Sql=@Sql+' AND  InquiryDate >= ''' + CAST(@FromDate AS VARCHAR(20)) + ''''
			END
		ELSE IF(@ToDate IS NOT NULL)
			BEGIN
				SET @Sql=@Sql+' AND  InquiryDate <= ''' + CAST(@ToDate AS VARCHAR(20)) + ''''
			END
			
			SET @Sql+= ' ORDER BY TC_CustomerId ASC,InquiryDate DESC'
			--print @Sql
			EXEC (@Sql)
			--print @Sql
END

