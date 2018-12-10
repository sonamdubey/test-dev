IF EXISTS (
    SELECT * FROM sysobjects WHERE id = object_id(N'[dbo].[TC_GetInqLeads]') 
    AND xtype IN (N'P')
)
    DROP PROCEDURE [dbo].[TC_GetInqLeads]
GO
	-- Modified By:	Suri on date filter is modified on 17 Jan 2013
-- Modified By:	Binumon George on 11th Apr 2012 Desc: added condition for missed and pending follow up
--===========================================================================
-- Author:		Binumon George
-- Create date: 08th Feb 2011
-- Description:	For Listing assigned and  Un assigned leads in Excel sheet
--TC_GetInqLeads 5,null,null,null,null,6,null,null,1,null,null,1,0,0,NULL
-- =============================================
CREATE PROCEDURE [dbo].[TC_GetInqLeads]
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
@ToDate DATETIME=NULL,
@IsWorkSheet BIT=NULL,--checking worksheet or not
@SrcLink TINYINT=NULL,--For missed call and pending call
@IsSuperAdmin BIT=NULL,--checking here super admin or normal user
@FollowupStatus TINYINT --Checking fresh lead or not
--@IsSingleUser BIT
AS
BEGIN
SET NOCOUNT ON
 DECLARE @Sql VARCHAR(MAX)
 DECLARE @IsSingleUser BIT=0--For checking worksheet only or both
	--SET @FromDate=GETDATE()-20
	--SET @ToDate=GETDATE()
	
	--checking here single user or not
	SELECT @IsSingleUser = DC.isWorksheetOnly FROM TC_DealerConfiguration DC WHERE  DC.DealerId=@BranchId
	SET @Sql='SELECT * from vwTC_InquiriesForExcel WHERE BranchId ='
	
		SET @Sql=@Sql+ CAST(@BranchId AS VARCHAR(5))
		IF(@IsSuperAdmin=1)
			BEGIN
				IF(@AssignType = 0 AND @SrcLink=3)
					BEGIN
						SET @Sql += ' AND TC_UserId IS NOT NULL'--inq Status from dashboard
					END
				ELSE IF(@AssignType = 0)--checking here lead assigned or not
					BEGIN
						--here not aassined so userid null
						SET @Sql += ' AND TC_UserId IS NULL'
					END
				ELSE IF(@AssignType != 0 AND @SrcLink IS NULL)-- checking here directly click on worksheet and inquiries in front end. if yes going in condition
					BEGIN
					--here assined so userid null. it taking all users leads for super admin
					IF(@IsSingleUser!=1 AND @IsWorkSheet=0)
						BEGIN
							IF(@UserId IS NULL)
							BEGIN
								SET @Sql += ' AND TC_UserId IS NOT NULL'
							END
							ELSE
							BEGIN
									SET @Sql += ' AND TC_UserId=' + CAST(@UserId AS VARCHAR(5))
							END
						END
						ELSE IF(@IsSingleUser!=1 AND @IsWorkSheet=1)
						BEGIN
							SET @Sql += ' AND TC_UserId ='+  CAST(@UserId AS VARCHAR(5))
						END
				END
			END
		ELSE
			BEGIN
			 IF(@AssignType != 0 AND @IsSingleUser=0)
				 BEGIN
					SET @Sql += ' AND TC_UserId= ' +  CAST(@UserId AS VARCHAR(5))
				 END
			END
		
		IF(@StatusuId IS NOT NULL)
			BEGIN
				SET @Sql=@Sql+' AND TC_InquiriesFollowupActionId=' + CAST(@StatusuId AS VARCHAR(5))
			END
			--ELSE
		--	BEGIN
			--	SET @Sql=@Sql+' AND TC_InquiriesFollowupActionId IN(1,2,7)'
			--END
			-- for followup and missed follow up
			IF(@StatusuId IS NULL AND @SrcLink IS NOT NULL)
				BEGIN
					SET @Sql=@Sql+' AND TC_InquiriesFollowupActionId IN(1,2,7)'
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
			/*
		IF(@UserId IS NOT NULL)
			BEGIN
				SET @Sql=@Sql+' AND TC_UserId =' + CAST(@UserId AS VARCHAR(5))
			END
			*/
		IF(@SourceId IS NOT NULL)
			BEGIN
				SET @Sql=@Sql+' AND SourceId =' + CAST(@SourceId AS VARCHAR(5))
			END
		IF(@LeadType IS NOT NULL)
			BEGIN
				SET @Sql=@Sql+' AND TC_InquiryTypeId =' + CAST(@LeadType AS VARCHAR(5))
			END
			
		IF(@FollowupStatus IS NOT NUlL)--Geting status of leads like fresh,All,Followup
			BEGIN
				SET @Sql=@Sql+ ' AND IsActionTaken ='+ CAST(@FollowupStatus AS VARCHAR(3))
			END
		IF(@IsWorkSheet !=1)-- For Inquiry
			BEGIN
				IF(@SrcLink IS NOT NULL AND @SrcLink=0)--missed call for inquiry
					BEGIN
						IF(@isSuperAdmin=1)
							BEGIN
								IF(@AssignType != 0)
									BEGIN
										SET @Sql=@Sql+' AND  NextFollowUpDate <'''+ CONVERT(NCHAR(10), GETDATE(), 120) +' 00:01'' AND TC_UserId IS NOT NULL'
									END
									ELSE
									BEGIN
										SET @Sql=@Sql+' AND  NextFollowUpDate <'''+ CONVERT(NCHAR(10), GETDATE(), 120) +' 23:59'''
									END
							END
							ELSE
							BEGIN
								SET @Sql=@Sql+' AND  NextFollowUpDate <'''+ CONVERT(NCHAR(10), GETDATE(), 120) +' 23:59'' AND TC_UserId= ' +  CAST(@UserId AS VARCHAR(5))
							END
					END
				ELSE IF(@SrcLink IS NOT NULL AND @SrcLink=1)--pending call work sheet
					BEGIN
						IF(@IsSuperAdmin=1)
							BEGIN
								IF(@UserId IS NULL AND @AssignType=0)
									BEGIN
										SET @Sql=@Sql+' AND  NextFollowUpDate >='''+ CONVERT(NCHAR(10), GETDATE(), 120) +' 00:01'' AND TC_UserId IS NULL'
									END
									ELSE
									BEGIN
										SET @Sql=@Sql+' AND  NextFollowUpDate >='''+ CONVERT(NCHAR(10), GETDATE(), 120) +' 00:01'' AND TC_UserId IS NOT NULL'
									END
							END
							ELSE
							BEGIN
								SET @Sql=@Sql+' AND  NextFollowUpDate >='''+ CONVERT(NCHAR(10), GETDATE(), 120) +' 00:01'' AND TC_UserId= ' +  CAST(@UserId AS VARCHAR(5))
							END
					END	
				ELSE IF(@FromDate IS NOT NULL AND @ToDate IS NOT NULL )
					BEGIN
						SET @Sql=@Sql+' AND  InquiryDate BETWEEN ''' + CAST(@FromDate AS VARCHAR(12)) + '00:01 AM'' AND ''' + CAST(@ToDate AS VARCHAR(12))+'11:59 PM'''  
					END
				ELSE IF(@FromDate IS NOT NULL)
					BEGIN
						SET @Sql=@Sql+' AND  InquiryDate >= ''' + CAST(@FromDate AS VARCHAR(12)) + '00:01 AM'''
					END
				ELSE IF(@ToDate IS NOT NULL)
					BEGIN
						SET @Sql=@Sql+' AND  InquiryDate <= ''' + CAST(@ToDate AS VARCHAR(12)) + '11:59 PM'''
					END
					
					
					SET @Sql+= ' ORDER BY TC_CustomerId ASC,InquiryDate DESC'
			END
		ELSE -- For worksheet
			BEGIN
				
				IF(@SrcLink IS NOT NULL AND @SrcLink=0)--missed call for worksheet
					BEGIN
						IF(@IsSuperAdmin=1 OR  @IsSingleUser=1)--super admin or sigle user
							BEGIN
								SET @Sql=@Sql+' AND  NextFollowUpDate <'''+ CONVERT(NCHAR(10), GETDATE(), 120) +' 23:59'''--' AND IsActionTaken='+ CAST(@FollowupStatus AS VARCHAR(3))
							END
							ELSE
							BEGIN
								SET @Sql=@Sql+' AND  NextFollowUpDate <'''+ CONVERT(NCHAR(10), GETDATE(), 120) +' 23:59'''--' AND IsActionTaken=' + CAST(@FollowupStatus AS VARCHAR(3))+' AND TC_UserId= ' +  CAST(@UserId AS VARCHAR(5))
							END
					END
				ELSE IF(@SrcLink IS NOT NULL AND @SrcLink=1)--pending call work sheet
					BEGIN
						IF(@IsSuperAdmin=1 OR @IsSingleUser=0)
							BEGIN
								SET @Sql=@Sql+' AND  NextFollowUpDate >='''+ CONVERT(NCHAR(10), GETDATE(), 120) +' 00:01'''--' AND IsActionTaken='+ CAST(@FollowupStatus AS VARCHAR(3))
							END
							ELSE
							BEGIN
								SET @Sql=@Sql+' AND  NextFollowUpDate >='''+ CONVERT(NCHAR(10), GETDATE(), 120) +' 00:01'''--' AND IsActionTaken='+ CAST(@FollowupStatus AS VARCHAR(3))+' AND TC_UserId= ' +  CAST(@UserId AS VARCHAR(5))
							END
					END
					
				ELSE IF(@FromDate IS NOT NULL AND @ToDate IS NOT NULL)
					IF(@FollowupStatus=1) -- added on 17 Jan 2013				
					BEGIN
						SET @Sql=@Sql+' AND  ((NextFollowUpDate BETWEEN ''' + CAST(@FromDate AS VARCHAR(12)) + ' 00:01 AM'' AND ''' + CAST(@ToDate AS VARCHAR(12))+'11:59 PM''))'  
					END
					ELSE -- added on 17 Jan 2013	
					BEGIN
						SET @Sql=@Sql+' AND  ((InquiryDate BETWEEN ''' + CAST(@FromDate AS VARCHAR(12)) + ' 00:01 AM'' AND ''' + CAST(@ToDate AS VARCHAR(12))+'11:59 PM''))'  
					END
				ELSE IF(@FromDate IS NOT NULL)					
					IF(@FollowupStatus=1) -- added on 17 Jan 2013
					BEGIN
						SET @Sql=@Sql+' AND  ((NextFollowUpDate >= ''' + CAST(@FromDate AS VARCHAR(12)) + ' 00:01 AM''))'
					END
					ELSE
					BEGIN
						SET @Sql=@Sql+' AND  ((InquiryDate >= ''' + CAST(@FromDate AS VARCHAR(12)) + ' 00:01 AM''))'
					END
				ELSE IF(@ToDate IS NOT NULL )
					IF(@FollowupStatus=1) -- added on 17 Jan 2013
					BEGIN
						SET @Sql=@Sql+' AND  ((NextFollowUpDate <= ''' + CAST(@ToDate AS VARCHAR(12)) + '11:59 PM''))'
					END
					ELSE
					BEGIN
						SET @Sql=@Sql+' AND  ((InquiryDate <= ''' + CAST(@ToDate AS VARCHAR(12)) + '11:59 PM''))'
					END
				ELSE IF(@FromDate IS NULL AND @ToDate IS  NULL  AND (@SrcLink <2 OR @SrcLink IS NULL))
					BEGIN
						SET @Sql=@Sql+' AND  (NextFollowUpDate IS NULL OR (NextFollowUpDate BETWEEN ''' + CONVERT(VARCHAR,CONVERT(DATE, GETDATE())) + ' 00:01 AM'' AND '''+ CONVERT(VARCHAR,CONVERT(DATE, GETDATE())) + ' 11:59 PM''' +'))'
					END
					/*
					IF(@StatusuId IS NULL AND @SrcLink IS NOT NULL)
						BEGIN
							SET @Sql=@Sql+' AND TC_InquiriesFollowupActionId IN(1,2,7)'
						END
					*/
					SET @Sql+= ' ORDER BY TC_CustomerId ASC,NextFollowUpDate DESC'
					
			END
			--PRINT @Sql
			EXEC (@Sql)
			--PRINT @SQL
			
END