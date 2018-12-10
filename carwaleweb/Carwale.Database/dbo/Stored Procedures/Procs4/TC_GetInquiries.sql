IF EXISTS (
    SELECT * FROM sysobjects WHERE id = object_id(N'[dbo].[TC_GetInquiries]') 
    AND xtype IN (N'P')
)
    DROP PROCEDURE [dbo].[TC_GetInquiries]
GO

	-- =============================================
-- Author:		Binumon George
-- Create date: 14th Dec 2011
-- Description:	comma seprated items made in unique column
-- =============================================
-- =============================================
-- Author:		Binumon George
-- Create date: 21th November 2011
-- Description:	For Listing entire purchase Inquiry in Excel sheet
-- =============================================
CREATE PROCEDURE [dbo].[TC_GetInquiries]
@BranchId INT,
@StatusuId INT=NULL,
@MakeId INT=NULL,
@ModelId INT=NULL,
@FDate VARCHAR(25)=NULL,
@EDate VARCHAR(25)=NULL,
@SourceId INT=NULL
AS
BEGIN
 DECLARE @Sql VARCHAR(max)
	SET @Sql='Select * From (Select  Row_Number() Over (Order By RequestDateTime DESC) AS RowN, 
			Cust.CustomerName, Cust.Mobile,Cust.Email, PInq.StockId, PInq.InterestedIn, 
			PInq.FollowUp As FollowUp, LOWER(InqS.Status) ''statusType'',
			convert(varchar(11), PInq.RequestDateTime) As RequestDateTime, Src.Source,convert(varchar, St.RegNo) As RegNo,
			SUBSTRING(CONVERT(VARCHAR(11), MakeYear, 113), 4, 8) As MakeYear,convert(varchar,St.Kms) AS Kms,
			convert(varchar,st.price) AS Price,St.Colour,
			(Ma.Name +'' ''+ Mo.Name +'' ''+ Vs.Name) Car
			From  TC_PurchaseInquiries PInq  Inner Join TC_CustomerDetails Cust
			On Cust.Id = PInq.CustomerId Inner Join TC_InquirySource Src On Src.Id = PInq.SourceId 
			LEFT JOIN TC_InquiryStatus InqS On PInq.InquiryStatusId=InqS.Id  
			LEFT JOIN TC_Stock St ON PInq.StockId = St.Id LEFT JOIN CarVersions Vs ON Vs.ID = St.VersionId
			LEFT JOIN CarModels Mo ON Mo.ID = Vs.CarModelId LEFT JOIN CarMakes Ma ON Ma.ID = Mo.CarMakeId 
			Where  PInq.BranchId='
		
		SET @Sql=@Sql+ CAST(@BranchId AS VARCHAR(5))
		IF(@StatusuId IS NOT NULL)
			BEGIN
				SET @Sql=@Sql+' AND PInq.InquiryStatusId=' + CAST(@StatusuId AS VARCHAR(2))
			END
			/*
		ELSE
			BEGIN
				SET @Sql=@Sql+'  And PInq.IsActionTaken=0'
			END
			*/
		IF(@SourceId IS NOT NULL)
			BEGIN
				SET @Sql=@Sql+' AND PInq.SourceId =' + CAST(@SourceId AS VARCHAR(2))
			END	
		IF(@MakeId IS NOT NULL)
			BEGIN
				SET @Sql=@Sql+' And Ma.Id =' + CAST(@MakeId AS VARCHAR(5))
			END
		IF(@ModelId IS NOT NULL)
			BEGIN
				SET @Sql=@Sql+' And Mo.Id =' + CAST(@ModelId AS VARCHAR(5))
			END
		IF(@FDate IS NOT NULL)
			BEGIN
				SET @Sql=@Sql+' And PInq.RequestDateTime BETWEEN '''+  CAST(@FDate AS VARCHAR(25))+''' AND ''' + CAST(@EDate AS VARCHAR(25))+''''
			END
			SET @Sql=@Sql+'  And PInq.IsActionTaken=0'	
				SET @Sql=@Sql+' ) AS TopRecords'
				--print @sql
				EXEC (@Sql)
END