IF EXISTS(
SELECT *
   FROM sys.views
     WHERE schema_id = SCHEMA_ID('dbo'))
     name = 'vwImagedetails' AND
     DROP VIEW dbo.vwImagedetails
GO

	





























CREATE View [dbo].[vwImagedetails]
as
select ID,'Con_RoadTestPages' as Table_Name,'/rt/'+cast(RTId as varchar(100))+'/'  as DirPath,MainImgPath as Filename,HostURL from Con_RoadTestPages  where IsReplicated=0  
union ALL
select ID,'CarPhotos' as Table_Name,DirectoryPath as DirPath,ISNULL(ImageUrlFull,'')+'$'+ISNULL(ImageUrlThumb,'')+'$'+ISNULL(ImageUrlThumbSmall,'') as Filename,HostURL from CarPhotos  where IsReplicated=0 And IsActive = 1
union ALL
--select ID,'Dealer_NewCar' as Table_Name,DirectoryPath as DirPath,ISNULL(ImageFull,'')+'$'+ISNULL(ImageThumb,'') as Filename,HostURL from Dealer_NewCar  where IsReplicated=0  
--union ALL
--select ID,'NCD_PhotoBanner' as Table_Name,DirectoryPath as DirPath,ISNULL(ImageFull,'')+'$'+ISNULL(ImageThumb,'') as Filename,HostURL from NCD_PhotoBanner  where IsReplicated=0  
--union ALL
--select ID,'NCD_Photos' as Table_Name,DirectoryPath as DirPath,ISNULL(ImageFull,'')+'$'+ISNULL(ImageThumb,'') as Filename,HostURL from NCD_Photos  where IsReplicated=0  
--union ALL
select ID,'TC_CarPhotos' as Table_Name,DirectoryPath as DirPath,ISNULL(ImageUrlFull,'')+'$'+ISNULL(ImageUrlThumb,'')+'$'+ISNULL(ImageUrlThumbSmall,'') as Filename,HostURL from TC_CarPhotos  where IsReplicated=0   And IsActive = 1
union ALL
select ID,'TC_SellCarPhotos' as Table_Name,DirectoryPath as DirPath,ISNULL(ImageUrlFull,'')+'$'+ISNULL(ImageUrlThumb,'')+'$'+ISNULL(ImageUrlThumbSmall,'') as Filename,HostURL from TC_SellCarPhotos  where IsReplicated=0   And IsActive = 1
union ALL
select ID,'Microsite_Images' as Table_Name,DirectoryPath as DirPath,ISNULL(ThumbImage,'')+'$'+ISNULL(LargeImage,'') as Filename,
HostURL from Microsite_Images  where IsReplicated=0 And IsActive = 1
union ALL
--select ID,'AW_Categories' as Table_Name ,' ' as DirPath,ImageName as Filename,HostURL from AW_Categories  where IsReplicated=0  
--union ALL
select ID,'CarBodyStyles' as Table_Name ,'/CarBodyStyles/' as DirPath,ImageUrl as Filename,HostURL from CarBodyStyles  where IsReplicated=0  
union ALL
select ID,'CarGalleryPhotos' as Table_Name ,'/cg/'+cast(modelId as varchar(100)) as DirPath,PhotoURL as Filename,HostURL from CarGalleryPhotos  where IsReplicated=0  
union ALL
select ID,'CarMakes' as Table_Name ,'/CarMakes/' as DirPath,LogoUrl as Filename,HostURL from CarMakes  where IsReplicated=0  
union ALL
select ID,'CarModels' as Table_Name ,'/cars/' as DirPath,ISNULL(SmallPic,'')+'$'+ISNULL(LargePic,'') as Filename,HostURL from CarModels  where IsReplicated=0   and LEN(ISNULL(SmallPic+'$'+LargePic,''))>1
union ALL
select ID,'CarVersions' as Table_Name ,'/cars/' as DirPath,ISNULL(SmallPic,'')+'$'+ISNULL(LargePic,'') as Filename,HostURL from CarVersions  where IsReplicated=0  and LEN(ISNULL(SmallPic+'$'+LargePic,''))>1
union ALL
--select ID,'Classified_CertifiedOrg' as Table_Name ,' ' as DirPath,LogoUrl as Filename,HostURL from Classified_CertifiedOrg  where IsReplicated=0  
--union ALL
--select ID,'CMS_Campaigns' as Table_Name ,' ' as DirPath,ROFilePath as Filename,HostURL from CMS_Campaigns  where IsReplicated=0  
--union ALL
--select ID,'Con_ArticlePages' as Table_Name ,' ' as DirPath,MainImgPath as Filename,HostURL from Con_ArticlePages  where IsReplicated=0  
--union ALL
--select ID,'Con_EditCms_Images' as Table_Name ,'/ec/'+cast(BasicId as varchar(100))+'/' as DirPath,cast(ID as varchar(100)) as Filename,HostURL from Con_EditCms_Images  where IsReplicated=0  
--union ALL
select ID,'Con_RoadTest' as Table_Name ,'/rt/'+cast(Id as varchar(100))+'/' as DirPath,MainImgPath as Filename,HostURL from Con_RoadTest  where IsReplicated=0  
union ALL
--select ID,'CW_PressReleases' as Table_Name ,' ' as DirPath,AttachedFile as Filename,HostURL from CW_PressReleases  where IsReplicated=0  
--union ALL
--select ID,'Dealers' as Table_Name ,' ' as DirPath,LogoUrl as Filename,HostURL from Dealers  where IsReplicated=0  
--union ALL
--select ID,'ExpectedCarLaunches' as Table_Name ,' ' as DirPath,PhotoName as Filename,HostURL from ExpectedCarLaunches  where IsReplicated=0  
--union ALL
--select ID,'Financers' as Table_Name ,' ' as DirPath,FinancerLogo as Filename,HostURL from Financers  where IsReplicated=0  
--union ALL
select Inquiryid as ID,'LiveListings' as Table_Name ,' ' as DirPath,FrontImagePath as Filename,HostURL from LiveListings  where IsReplicated=0 And ISNULL(FrontImagePath, '') != ''
 union ALL
--select ID,'ServiceProviderBranchs' as Table_Name ,' ' as DirPath,PhotoURL as Filename,HostURL from ServiceProviderBranchs  where IsReplicated=0  
--union ALL
--select ID,'ShowRoomPhotos' as Table_Name ,'/dealer/img/' as DirPath,ISNULL(Thumbnail,'')+'$'+ISNULL(LargeImage,'') as Filename,HostURL from ShowRoomPhotos  where IsReplicated=0 
select ID,'ShowRoomPhotos' as Table_Name ,
case ImageCategory when 3 then '/dealer/img/HeaderImages/' else '/dealer/img/' end as DirPath,
case ImageCategory when 3 then ISNULL(Thumbnail,'') else ISNULL(Thumbnail,'')+'$'+ISNULL(LargeImage,'') end as Filename,
HostURL from ShowRoomPhotos  where IsReplicated=0 
 union ALL
select ID,'Con_RoadTestAlbum' as Table_Name ,'/rt/'+CAST(RTId as varchar(10))+'/' as DirPath,ISNULL(CAST(Id as varchar(10)),'') as Filename,HostURL from Con_RoadTestAlbum  where IsReplicated=0 
union all
--select ID,'Con_FeaturedListings' as Table_Name ,'/featuredcar/' as DirPath,ISNULL(CAST(Id as varchar(10)),'') as Filename,HostURL from Con_FeaturedListings  where IsReplicated=0 
select ID,'Con_FeaturedListings' as Table_Name ,'/featuredcar/' as DirPath,case isnull(id,0) when 0 then '' else (CAST(Id as varchar(10)))+'.jpg' end as Filename,HostURL from Con_FeaturedListings  where IsReplicated=0 
union all
 select u.ID,'UP_Photos' as Table_Name ,'/c/p/'+CAST(a.userid as varchar(10))+'/' as DirPath,(case Size500 when 1 then u.Name+'_500.jpg' else '' end)+'$'+(case Size800 when 1 then u.Name+'_800.jpg' else '' end)+'$'+(case Size1024 when 1 then u.Name+'_1024.jpg' else '' end) as Filename ,u.HostURL from UP_Photos as U  join UP_Albums as a on u.AlbumId=a.ID  where u.IsReplicated=0 
union all
 select ID,'Wallpapers' as Table_Name ,(case Eight when 1 then '/wp/800/' else '' end)+'$'+(case Thousand when 1 then '/wp/1000/' else '' end) as DirPath,RandomString+'.jpg' as Filename,HostURL from wallpapers  where IsReplicated=0 
 union all
 select TC_Bug_Id as ID,'TC_BugFeedback' as Table_Name , '/tc/bugs/' as DirPath,REPLACE(filepath,'/tc/bugs/','') as Filename,HostURL from TC_BugFeedback  where IsReplicated=0 
 union all
 select Id as ID,'Acc_Items' as Table_Name , '/accessories/items/' as DirPath,CAST(id as varchar(50)) as Filename,HostURL from Acc_Items  where IsReplicated=0 and IsActive=1
union all
select Id as ID,'Acc_ItemsAdditionalImages' as Table_Name , '/accessories/items/' as DirPath,AddionalImagesLocation as Filename,HostURL from Acc_ItemsAdditionalImages  where IsReplicated=0 
union all
select Authorid as ID,'Con_EditCms_Author' as Table_Name , '/ec/authorImg/'+ cast(Authorid as varchar(50)) as DirPath,isnull(PicName+'.jpg','') as Filename,HostURL from Con_EditCms_Author  where IsReplicated=0 
union all
select Id as ID,'UserProfile' as Table_Name , '/c/up/a/$/c/up/r/' as DirPath,AvtarPhoto+'$'+RealPhoto as Filename,HostURL from UserProfile  where IsReplicated=0 and LEN(ISNULL(AvtarPhoto+'$'+RealPhoto,''))>1
union ALL 
--select ID,'Con_EditCms_Basic' as Table_Name ,'/ec/'+cast(Id as varchar(100))+'/' as DirPath,cast(ID as varchar(100)) as Filename,HostURL from Con_EditCms_Basic  where IsReplicated=0  
--union ALL
select ID,'Con_EditCms_Images' as Table_Name ,
Case When HasCustomImg = 1 Then 
	Case When IsMainImage = 1 Then '/ec/' + CONVERT(Varchar, BasicId) + '/img/m/' + ImageName + '$' + ImagePathThumbnail + '$' + ImagePathLarge  + '$' + ImagePathOriginal + '$' + ImagePathCustom
	Else ImagePathThumbnail + '$' + ImagePathLarge  + '$' + ImagePathOriginal + '$' + ImagePathCustom End
Else 
	Case When IsMainImage = 1 Then '/ec/' + CONVERT(Varchar, BasicId) + '/img/m/' + ImageName + '$' + ImagePathThumbnail + '$' + ImagePathLarge  + '$' + ImagePathOriginal
	Else ImagePathThumbnail + '$' + ImagePathLarge  + '$' + ImagePathOriginal End
End as DirPath,
cast(id as varchar(100)) As Filename,
HostURL from Con_EditCms_Images where IsReplicated=0 and IsActive=1
--union ALL
--select ID,'Microsite_Images' as Table_Name,DirectoryPath as DirPath,ISNULL(ThumbImage,'')+'$'+ISNULL(LargeImage,'') as Filename,
--HostURL from Microsite_Images  where IsReplicated=0 And IsActive = 1
--union ALL
--select ID,'Con_EditCms_Basic' as Table_Name ,'/ec/'+cast(Id as varchar(100))+'/' as DirPath,
--case HasCustomImg when 1 then cast(ID as varchar(100))+'.jpg$'+cast(ID as varchar(100))+'_m.jpg$'+cast(ID as varchar(100))+'_l.jpg$'+cast(ID as varchar(100))+'_ol.jpg$'+cast(ID as varchar(100))+'_c.jpg' 
--when 0 then cast(ID as varchar(100))+'.jpg$'+cast(ID as varchar(100))+'_m.jpg$'+cast(ID as varchar(100))+'_l.jpg$'+cast(ID as varchar(100))+'_ol.jpg' 
--end as Filename,HostURL from Con_EditCms_Basic where IsReplicated=0 
union all
Select Id, 'Con_CarComparisonList' As Table_Name, '/research/carcomparison/' As DirPath, CONVERT(VarChar, VersionId1) + '_' + CONVERT(VarChar, VersionId2) + '.jpg' As FileName, HostURL
From Con_CarComparisonList Where IsReplicated = 0 And IsActive = 1 And HostURL Is Not Null

























