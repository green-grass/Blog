using System;
using System.Linq;
using Microsoft.AspNet.Builder;
using Microsoft.AspNet.Hosting;
using Microsoft.Extensions.DependencyInjection;
using PhantomNet.Blog.JDev.Models;

namespace PhantomNet.Blog.JDev
{
    partial class Startup
    {
        private void SeedData(IApplicationBuilder app, IHostingEnvironment env)
        {
            if (!env.IsDevelopment())
            {
                return;
            }

            using (var context = app.ApplicationServices.GetRequiredService<BlogDbContext>())
            {
                if (context.Categories.Count() == 0)
                {
                    context.Categories.AddRange(new Category[] {
                            new Category { IsActive = true, Language = "en", Name = "Hotels",    UrlFriendlyName = "hotels"    },
                            new Category { IsActive = true, Language = "en", Name = "Tours",     UrlFriendlyName = "tours"     },
                            new Category { IsActive = true, Language = "en", Name = "Food",      UrlFriendlyName = "food"      },
                        });
                    context.SaveChanges();
                }

                if (context.Articles.Count() == 0)
                {
                    context.Articles.AddRange(new Article[] {
                            new Article
                            {
                                IsActive = true, Language = "en", PublishDate = DateTime.Now.AddDays(-1),
                                Author = "Minh Nguyễn",
                                Title = "Ai Sắp Đi Hội An, Nhớ Ghi Chú 6 Quán Cafe Mới Và Cực Xinh Này!",
                                UrlFriendlyTitle = "cafe-hoi-an",
                                ShortContent = "Nếu đã bắt đầu nhớ Hội An rồi, hãy lên kế hoạch đi thôi bởi Hội An vừa có thêm bao nhiêu hàng quán mới, vừa xinh, vừa lạ đấy.",
                                Content = @"Hội An rất kỳ lạ, rõ ràng chỉ nhỏ như nắm tay, cũng chẳng có mấy trò vui và mới lạ, thế mà bao nhiêu người trẻ, cứ dăm ba tháng lại bắt đầu tặc lưỡi, chép miệng: ""Nhớ Hội An mất rồi"".<br /><br />
Thế Hội An có gì? Đồ ăn ngon, những con phố cổ im lìm trong nắng vàng xứ Quảng, có mùi nhang bay lẫn trong không khí vào buổi tinh mơ rất bình yên, sông Hoài lững lờ trôi mà chỉ cần thả mình trên một chiếc thuyền rồi nhìn ra hai bên bờ - bạn cũng thấy nhẹ lòng. Nhưng một đặc điểm nữa rất giới trẻ, rất phù phiếm mà hay ho, ấy là Hội An có quá nhiều quán cafe, quán ăn chất - xinh - ngon. Thế mới bảo, những kẻ yêu Hội An, toàn là lũ thích ăn ngon, thích sống chậm, chỉ ngồi đờ ra rồi nhìn ngắm thời gian trôi, reo lên thích thú vì quán này có góc nhỏ xinh khác lạ.<br /><br />
Trước đây đến Hội An có Hải, có Before & Now, có Triết, gần hơn có Reaching Out. Nhưng khoảng nửa năm trở lại đây, Hội An mới rồi, có thêm nhiều quán xinh xắn, mới mẻ hơn. Những ai chưa đi Hội An một năm thôi cũng sẽ phải tò mò, phải háo hức để lên kế hoạch quay lại miền đất này, chỉ để thoả mãn cái thú nhã nhặn, phong lưu là ngồi cafe đẹp và hít hà mùi bình yên."
                            },
                            new Article
                            {
                                IsActive = true, Language = "en", PublishDate = DateTime.Now.AddDays(-1),
                                Author = "Minh Nguyễn",
                                Title = "Sầm Sơn - Biển Thức Giấc Chào Mùa Hè 2016",
                                UrlFriendlyTitle = "bien-sam-son",
                                ShortContent = "Nằm ở đầu dải đất miền Trung, tỉnh Thanh Hóa anh hùng từ bao đời nay vẫn giữ cho mình những bản sắc rất riêng, những di tích lịch sử hào hùng và danh tiếng từ những người conn hiếu học của quê hương. Thêm vào đó, Thanh Hóa còn sở hữu một bãi biển đẹp, hiện đại bậc nhất miền Trung, đó chính là biển Sầm Sơn.",
                                Content = @"Là bãi biển đẹp nằm ở dải đất cuối cùng của dãy núi Trường Lệ, Sầm Sơn mang vẻ sơn thủy hữu tình hấp dẫn du khách. Sầm Sơn được làm du lịch từ những năm đầu của thế kỷ 20, thời mà các quan chức người Pháp lẫn vua quan nhà Nguyễn đều mê mẩn nơi sóng vỗ bờ cát mịn dài hơn 10km này.<br /><br />
Kể từ khi nâng cấp lên thị xã, Sầm Sơn ngày càng thu hút khách du lịch nhờ cải tiến nhiều dịch vụ, đường sá, các công trình hiện đại hơn nhằm phục vụ nhu cầu ngày một lớn của khách trong và ngoài nước.<br /><br />
Nằm cách TP Thanh Hóa 15km, cách thủ đô Hà Nội chừng 160km, Sầm Sơn có vị trí thuận lợi để trở thành một địa điểm lý tưởng cho các cơ quan, đoàn thể gần xa lựa chọn làm điểm du lịch, tổ chức team building, Gala Dinner, các trò chơi mang tính đồng đội khác… ngay trên bãi biển xinh đẹp này."
                            },
                            new Article
                            {
                                IsActive = true, Language = "en", PublishDate = DateTime.Now.AddDays(-1),
                                Author = "Đình Phong",
                                Title = "Thử Thách Tú Làn 2016",
                                UrlFriendlyTitle = "tu-lan-2016",
                                ShortContent = "100 người chơi chia làm 10 đội cùng chèo thuyền, vượt núi giữa rừng rậm ở Quảng Bình để nỗ lực vượt qua chính bản thân, gắn kết và hỗ trợ đồng đội về đích.",
                                Content = @"Tú Làn Advanture Race được tổ chức từ 31/3 đến 3/4 tại huyện Minh Hóa (Quảng Bình). Chặng đua đầu tiên là chèo thuyền độc mộc 15 km trên sông Rào Nan.<br /><br />
Nhiều người chơi lần đầu chèo thuyền độc mộc nên để thuyền đi đúng hướng là vô cùng khó khăn. Về sau, các đội vừa đua vừa đếm từ 1 đến 10 rồi đổi bên chèo để giữ thuyền đi đúng hướng.<br /><br />
Nhiều trẻ em địa phương hứng thú, ra tận mỏm đá giữa sông để quan sát, cổ vũ và làm ""hoa tiêu"" cho các đội đua."
                            },
                            new Article
                            {
                                IsActive = true, Language = "en", PublishDate = DateTime.Now.AddDays(-1),
                                Author = "Minh Nguyễn",
                                Title = "Biển Và Núi - Hai Sắc Cảnh Làm Nên Bức Tranh Phú Yên Đẹp Không Tưởng",
                                UrlFriendlyTitle = "bien-nui-phu-yen",
                                ShortContent = "Khi đã đem lòng mê đắm điểm du lịch nào đó thì thực sự nơi đó phải đem đến cho du khách một vẻ đẹp quyến rũ hay cả những cảm xúc đặc biệt nhất. Phú Yên cũng vậy, cũng trở thành điểm đến vạn người mê bởi Phú Yên có một vẻ đẹp kỳ lạ. Phú Yên với những con người chất phác, thân tình và bức tranh hòa quyện giữa mây và núi đã khiến cho Phú yên dường như trở thành mảnh đất đẹp không tưởng!",
                                Content = @"Chỉ riêng sắc xanh của biển thì khó có thể làm nên được một Phú Yên đẹp không tưởng như vậy nếu thiếu đi sự chấm phát từ nét xanh lá của núi. Biển Phú Yên nằm bên cạnh núi, núi xanh trùng trùng điệp điệp bao bọc và chở che cho biển cả, còn biển thì luôn khép mình, vùi vào lòng núi Phú Yên. Núi là mẹ, là đấng thiên nhiên xanh rực rỡ chở che lấy những đứa con, nhưng có khi, biển lại chuyển mình làm mẹ, hào nhoáng dưới ánh nắng vàng, đón Phú Yên tươi trẻ trong ngày mới.<br /><br />
Vịnh Xuân Đài đẹp là vậy nhưng nếu không có mây núi che ngang, liệu còn giữ đẹp nét đẹp bình dị mà kiều diễm như nàng công chúa ngủ vùi ấy?  Không chỉ vậy, xung quanh vịnh Xuân Đài, trừ hướng tiếp giáp với biển đều được bao bọc với sắc xanh lá của núi và rừng:<br /><br />
""Non xanh, nước biếc như tranh họa đồ…""<br /><br />
Cây và đá chen chúc nhau, có mảnh xanh lơ, có mảng xanh lá cây đậm đà, cùng nhau phô mình trên đồi núi, khiến vịnh Xuân Đài trở thành bức tranh thiên nhiên hữu tình có cả biển và núi. Người du khách đến đây không chỉ mơ màng mà còn phải ngẩn ngơ trước vẻ đẹp tinh khiết của thiên nhiên.<br /><br />
Đèo Cả hay Núi Đá Bia lại bao bọc vũng Rô - một điểm đến mà du khách không thể bỏ qua khi đến với du lịch Phú Yên cảm nhận vẻ đẹp trọn vẹn của núi rừng và biển nơi đây. Chính sắc màu của núi rừng nơi đây lại làm nổi bật lên dưới chân nó những bãi cát trắng mịn, mặt biển xanh sâu lắng, hòa mình cùng ánh mặt trời chói chan ngày hè. Núi và biển ở Phú Yên, hoàn toàn không tách biệt, núi và biển là một, là vẻ đẹp không bao giờ bị nhạt phai."
                            },
                            new Article
                            {
                                IsActive = true, Language = "en", PublishDate = DateTime.Now.AddDays(-1),
                                Author = "Đình Phong",
                                Title = "10 Bãi Tắm 'Vạn Người Mê' Siêu Gần Hà Nội",
                                UrlFriendlyTitle = "bai-tam-gan-ha-noi",
                                ShortContent = "Khi mùa hè dần đến với tiết trời nóng bức, oi ả với những vạt nắng chói chang thì người người ở đất thủ đô và các tỉnh lân cận lại “đổ xô” tìm về những bãi tắm tuyệt đẹp để vừa thỏa thích đắm mình trong nước biển mát lạnh, vừa tha hồ ngắm cảnh thiên nhiên thơ mộng. Vậy nên, Mytour sẽ giúp bạn xua tan nóng bức, gạt phăng những phiền muộn và lấy lại cảm giác phấn chấn, sảng khoái cùng với 10 bãi tắm “vạn người mê” gần ngay đất thủ đô Hà Nội nhé!",
                                Content = @"Bãi Cát Cò là gồm 3 nổi tiếng nhất quần đảo Cát Bà ở xứ Hải Phòng, nối liền nhau bởi con đường nhỏ men theo triền núi. Đến với bãi Cát Cò, du khách sẽ được đắm mình trong làn nước trong xanh màu ngọc bích, đùa nghịch với những ngọn sóng gợn êm dịu, lang thang trên dải cát phẳng mịn. Bên cạnh đó, từ bãi tắm Cát Cò bạn sẽ còn được phóng tầm mắt ra xa ngắm nhìn cảnh vật đẹp miên man, vừa kỳ vĩ lại vừa thơ mộng. Khung cảnh ấy còn vẽ nên bức tranh thiên nhiên hữu tình, khiến bao người say đắm, ngất ngây mãi chẳng dứt.<br /><br />
Bãi Tùng Thu được mệnh danh là “bãi biển chuẩn quốc tế” với đầy đủ và đa dạng các dịch vụ du lịch và nghỉ dưỡng. Tại bãi tắm này, bạn không chỉ được khám phá vẻ đẹp của biển xanh mênh mông, thỏa thích bơi lội mà còn được đắm mình trong không gian vừa hiện đại, sang trọng vừa hài hòa với nét nguyên sơ của thiên nhiên. Bãi Tùng Thu hứa hẹn tạo ra những khoảnh khắc tuyệt vời, mang đến bạn những phút giây thư giãn bình yên và được trải nghiệm những hoạt động du lịch biển đầy thú vị.<br /><br />
Bãi Cát Dứa (nằm trên đảo Khỉ) là một bãi tắm tuyệt đẹp, đầy sức cuốn hút và quyến rũ du khách tìm về trong những dịp du lịch Cát Bà. Bãi tắm Cát Dứa mang vẻ đẹp đặc trưng của vịnh Lan Hạ với làn nước trong vắt, xanh xanh. Ở bãi tắm này, bạn còn được thỏa sức ngắm cảnh, chèo thuyền kayak, tham gia tour lặn biển đầy thú vị và rong chơi cùng những chú khỉ tinh nghịch trên dải cát mịn màng như thảm nhung trải dài quanh đảo."
                            },
                        });
                    context.SaveChanges();
                }

                if (context.Bloggers.Count() == 0)
                {
                    context.Bloggers.AddRange(new Blogger[] {
                            new Blogger { IsActive = true, PenName = "Minh Nguyễn",    UrlFriendlyPenName = "minh-nguyen"    },
                            new Blogger { IsActive = true, PenName = "Đình Phong",     UrlFriendlyPenName = "dinh-phong"     },
                        });
                    context.SaveChanges();
                }
            }
        }
    }
}
