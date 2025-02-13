// See https://aka.ms/new-console-template for more information

using memoryMapViewer;
using Microsoft.Win32;
using Raylib_cs;

using System.Numerics;
using System.Runtime.InteropServices;
using System.Text;
using System.Text.Json;
using static Raylib_cs.Raylib;

//Console.WriteLine("Hello, World!");
var screenWidth = 800;
var screenHeight = 480;
Raylib.SetConfigFlags(ConfigFlags.ResizableWindow);

InitWindow(800, 480, "MemoryMapViewer");

const int baseFontSize = 20;  // Font size at 1920x1080
const int referenceWidth = 1920;
const int referenceHeight = 1080;

List<LabeledSection<RegisterSection>> AllRegister = [];
List<LabeledSection<RomSection>> _AllLayouts = [];
LabeledSection<RomSection> _currentLayout = new();
List<MemorySection> _memorySections = [];
List<string> LabelList = [];
List<string> LabelListDynamic = [];
List<string> LabelListFiltered = [];
MemorySection? _currenMemorySection = null;
Camera2D camera = new();
camera.Target = new Vector2(128, 128);
camera.Rotation = 0.0f;
camera.Zoom = 2.0f;
camera.Offset = new Vector2((screenWidth / 2.0f) , (screenHeight / 2.0f));
Vector2 _oldMouseDragPos = Vector2.Zero;
Vector2 _mouseWorldPos = Vector2.Zero;

float _percentLoad = 0;
int _fontSize = 26;

int _OldAddress = -1;
bool _Dragging = false;
bool _SearchMode = false;
bool _PromptOn = false;
string _SearchString = "";
string _StringInfoPanel = "";
string _StringPopup = "";
byte _SelectedBank = 0;
ushort _SelectedAddress = 0;
HighLightRegister? _HighLight = null;
int _cursorSearchPosition = 0;
int _cursorSuggestionPosition = 0;
StringBuilder _sbtextBox = new();
StringBuilder _sbtextBox2 = new();
StringBuilder _inputText = new StringBuilder();

float _amplitude = 127.5f; // Amplitude of the sine wave (half of 255)
float _frequency = 1.0f;  // Frequency of the sine wave (1 cycle per second)
float _offset = 127.5f;

double last_time = Raylib.GetTime();

_AllLayouts.AddRange(JsonFileLoader.LoadAllJsonFiles<LabeledSection<RomSection>>("layouts/"));
_currentLayout = _AllLayouts.FirstOrDefault() ?? new();

AllRegister.AddRange(JsonFileLoader.LoadAllJsonFiles<LabeledSection<RegisterSection>>("register/"));

LabelList.AddRange(AllRegister.SelectMany(x => x.LayoutList.SelectMany(y => new[] { y.Label, y.LabelShort })));

bool _isloading = false;

SetTargetFPS(60);
int lastWidth = 0;
int lastHeight =0;
while (!WindowShouldClose())
{
    Update();
    Render();
}
CloseWindow();

void Update()
{
    int width = Raylib.GetScreenWidth();
    int height = Raylib.GetScreenHeight();

    // If window size changed, re-center the camera
    if (width != lastWidth || height != lastHeight)
    {
        camera.Offset = new System.Numerics.Vector2(width / 2, height / 2);
        lastWidth = width;
        lastHeight = height;
        float scaleFactor = MathF.Sqrt((width * height) / (float)(referenceWidth * referenceHeight));
        _fontSize = (int)(baseFontSize * scaleFactor);
        _fontSize = Math.Clamp(_fontSize, 20, 50); // Prevents extreme values
    }

    if (IsFileDropped())
    {
     
        var droppedFiles = GetDroppedFiles();
        var SymbolFile = "";
        var SymbolType = "";
        for (int i = 0; i < droppedFiles.Length; i++)
        {
            if (File.Exists(droppedFiles[i]))
            {
                var type = Path.GetExtension(droppedFiles[i]);
                if (type == ".sym")
                {
                    SymbolFile = droppedFiles[i];
                    SymbolType = type;
                    break;
                }
            }
        }

        if (string.IsNullOrEmpty(SymbolFile))
            return;

        _memorySections.Clear();
        string[] lines = File.ReadAllLines(SymbolFile);

        if (SymbolType == ".sym")
        {

            _memorySections.Clear();
            Task.Run(async () =>
            {
                _isloading = true;
                var res = await MemorySectionExtensions.LoadData(lines, Counter);
                _memorySections.AddRange(res);
            LabelListDynamic.AddRange(_memorySections.Select(x => x.Label));
                _isloading = false;
            });
            
        }
     
    }
    void Counter (int now, int all)
    {

        _percentLoad = now/ (float)all *100f;
       //Render();
    }




    if (IsKeyPressed(KeyboardKey.Enter))
    {
        if (_SearchMode)
        {

            var entryGT0 = Math.Min(10, LabelListFiltered.Count) > 0;
            if (entryGT0)
            {
                string res = LabelListFiltered[_cursorSuggestionPosition];
                _inputText.Clear();
                _inputText.Append(res);
                _cursorSearchPosition = _inputText.Length;
            }



            _HighLight = null;
            
            
            
            var searchText = _inputText.ToString();
            if (searchText.Length > 0)
            {
                var foundmemorySection = _memorySections.FirstOrDefault(x => x.Label.ToLower() == searchText.ToLower());
                if (foundmemorySection != null)
                {
                    _SelectedBank = (byte)foundmemorySection.Bank;
                    var t = CreateMemorySectionText(foundmemorySection);
                    _HighLight = new() { Address = (ushort)foundmemorySection.Address, Bank = (byte)foundmemorySection.Bank, Text = t };
                }
                else
                {
                    var foundRegister = AllRegister
                              .SelectMany(x => x.LayoutList)
                              .FirstOrDefault(y => y.Label.ToLower() == searchText.ToLower() || y.LabelShort.ToLower() == searchText.ToLower());
                    if (foundRegister != null)
                    {
                        _SelectedBank = 0;

                        var t = CreateRegisterText(foundRegister);
                        _HighLight = new() { Address = (ushort)foundRegister.Address, Bank = 0, Text = t };
                    }
                }
            }
        }

        _cursorSearchPosition = 0;
        _inputText.Clear();
        _SearchMode = !_SearchMode;
    }
    if (_SearchMode)
    {
        KeyboardTextMode();
        double current_time = GetTime();
        double elapsed_time = (current_time - last_time) * 1000;
        if (elapsed_time > 500)
        {
            last_time = current_time;
            _PromptOn = !_PromptOn;
        }
        return;
    }

    Vector2 mouseScreenPos = GetMousePosition();
    _mouseWorldPos = GetScreenToWorld2D(mouseScreenPos, camera);
    _SelectedAddress = (ushort)(Math.Clamp((int)_mouseWorldPos.X, 0x00, 0xFF) + (Math.Clamp((int)_mouseWorldPos.Y, 0x00, 0xFF) * 0x100));
    HandleZoom(ref camera, mouseScreenPos, _mouseWorldPos);
    HandleDragging(ref camera, ref _oldMouseDragPos, ref _Dragging, mouseScreenPos);

    if (IsKeyPressedRepeat(KeyboardKey.KpAdd) || IsKeyPressed(KeyboardKey.KpAdd))
    {
        ++_SelectedBank;
        _OldAddress = -1;
    }

    if (IsKeyPressedRepeat(KeyboardKey.KpSubtract) || IsKeyPressed(KeyboardKey.KpSubtract))
    {
        --_SelectedBank;
        _OldAddress = -1;
    }
    if (IsKeyPressedRepeat(KeyboardKey.Q) || IsKeyPressed(KeyboardKey.Q))
    {
        _currentLayout = GetNextItem(_AllLayouts, _currentLayout) ?? new();
        _OldAddress = -1;
    }
}
static T? GetNextItem<T>(List<T> list, T currentItem)
{
    int index = list.IndexOf(currentItem);
    if (index == -1 || list.Count == 0) return default; // Handle not found or empty list

    // Use modulus to wrap around
    return list[(index + 1) % list.Count];
}
static Camera2D HandleZoom(ref Camera2D camera, Vector2 mouseScreenPos, Vector2 mouseWorldPos)
{
    // Zoom in/out using mouse wheel
    float wheel = Raylib.GetMouseWheelMove();
    if (wheel != 0)
    {
        float zoomFactor = 1.0f + wheel * -.1f; // Increase or decrease zoom

        float scaleFactor = 1.0f + (0.25f * Math.Abs(wheel));
        if (wheel < 0) scaleFactor = 1.0f / scaleFactor;
        camera.Zoom = Math.Clamp(camera.Zoom * scaleFactor, 1.5f, 50.0f);

        // Get new world position of mouse after zoom
        Vector2 mouseWorldPosAfterZoom = Raylib.GetScreenToWorld2D(mouseScreenPos, camera);

        // Adjust camera target to maintain world-space position of mouse cursor
        camera.Target -= (mouseWorldPosAfterZoom - mouseWorldPos);
    }

    return camera;
}

static void HandleDragging(ref Camera2D camera, ref Vector2 oldMouseDragPos, ref bool dragging, Vector2 mouseScreenPos)
{
    if (Raylib.IsMouseButtonDown(MouseButton.Right))
    {
        Vector2 delta = GetMouseDelta();
        delta = delta * -1.0f / camera.Zoom;
        camera.Target += delta;
    }
}

void Render()
{
    BeginDrawing();
    ClearBackground(new Color(0x16, 0x14, 0x23));

    RenderBank();

    if (_isloading)
    {
        DrawRectangle(10, 10, GetScreenWidth() - 20, GetScreenHeight() - 20, new Color(30, 30, 30, 175));
        int x = MeasureText(_percentLoad.ToString(".0#"), _fontSize);
        DrawText(_percentLoad.ToString(".0#"), (GetScreenWidth() + x) >> 1, GetScreenHeight() >> 1, _fontSize, Color.White);


    }
    else
    {

        var boxSize = MeasureText(_StringInfoPanel, _fontSize);
        _sbtextBox2.Clear();
        _sbtextBox2.AppendLine($"Selected Bank: {_SelectedBank:X2}");
        _sbtextBox2.AppendLine($"Zoom: {camera.Zoom}");

        if (_currentLayout != null && _currentLayout.Name.Length > 0)
        {
            _sbtextBox2.AppendLine($"Address: {_SelectedAddress:X4}");
            _sbtextBox2.Append($"RomType: {_currentLayout.Name}");
        }
        else
        {
            _sbtextBox2.Append($"Address: {_SelectedAddress:X4}");
        }
        _StringInfoPanel = _sbtextBox2.ToString().Replace("\r", "");
        if (!_SearchMode)
        {
            int numLines = _StringInfoPanel.Split('\n').Length;

            DrawRectangle(10, 10, boxSize + 20, numLines * _fontSize + 20, new Color(15, 15, 15, 175));
            DrawRectangle(10, 10, boxSize + 20, numLines * _fontSize + 20, new Color(15, 15, 15, 175));
            DrawText(_StringInfoPanel, 15, 15, _fontSize, Color.RayWhite);

            if (_HighLight.HasValue)
                DrawHighLightBox(_fontSize, _HighLight.Value, ref boxSize, ref numLines);

            DrawPopupInfoV2();
        }
        else
        {
            var w = GetScreenWidth();
            var h = GetScreenHeight();

            DrawRectangle(0, 0, w, h, new Color(25, 25, 25, 175));
            DrawRectangle(0, h / 2, w, 26, new Color(5, 5, 5, 175));

            _SearchString = _inputText.ToString();
            var sw = MeasureText(_SearchString, _fontSize);
            var cw = MeasureText(_SearchString.Substring(0, _cursorSearchPosition), _fontSize);

            DrawText(_SearchString, w - sw >> 1, h >> 1, _fontSize, Color.White);
            if (_PromptOn)
                DrawRectangle((w + sw >> 1) - ((sw - cw)), h >> 1, 4, 26, new Color(200, 200, 200, 175));

            if (LabelListFiltered.Count > 0)
            {
                var entryCount = Math.Min(10, LabelListFiltered.Count);
                DrawRectangle(w >> 2, (h / 2) + _fontSize * 2, w >> 1, _fontSize * entryCount, new Color(30, 30, 30, 175));
                for (int i = 0; i < entryCount; i++)
                {
                    if (_cursorSuggestionPosition == i)
                        DrawText(LabelListFiltered[i], w >> 2, (h / 2) + _fontSize * i + _fontSize * 2, _fontSize, new Color(200, 233, 0, 200));
                    else
                        DrawText(LabelListFiltered[i], w >> 2, (h / 2) + _fontSize * i + _fontSize * 2, _fontSize, new Color(150, 150, 150, 175 - Math.Abs(_cursorSuggestionPosition - i) * 15));
                }
            }
        }
    }
    EndDrawing();
}
void RenderBank()
{
    var msBank = _memorySections.Where(x => x.Bank == _SelectedBank).ToArray();
    BeginMode2D(camera);

    //Background
    DrawRectangleGradientH(0, 0, 0x100, 0x100,
        new Color(0x3A, 0x2D, 0x6F),
        new Color(0x24, 0x55, 0x60));

    //RomType
    foreach (var item in _currentLayout.LayoutList)
    {
        if (_SelectedBank >= item.BankStart && _SelectedBank <= item.BankEnd)
        {
            var c = item.BlendColor;
            var y1 = item.SectionStart >> 8;
            var y2 = item.SectionEnd >> 8;
            DrawRectangle(0, y1, 0x100, y2 - y1 + 1, new Color(c.R, c.G, c.B, (byte)175));
        }
    }
    //Allcotations
    for (int i = 0; i < msBank.Length; i++)
    {
        RenderAllocations(msBank[i]);
    }
    //SNES Register
    if (_SelectedBank == 0x00)
    {
        foreach (var item in AllRegister)
        {
            RenderRegister(item);
        }
    }
    //HighLight
    if (_HighLight.HasValue && _SelectedBank == _HighLight.Value.Bank)
    {
        RenderHighlightPixel(_HighLight.Value, _amplitude, _frequency, _offset);
    }

    EndMode2D();
}
static void RenderRegister(LabeledSection<RegisterSection> registers)
{
    foreach (var register in registers.LayoutList)
    {
        var x = register.Address % 0x100;
        var y = register.Address / 0x100;
        var width = 1;
        var dark = (x & 1) == 0;

        Color c = new Color(registers.Color.R, registers.Color.G, registers.Color.B, (byte)175);
        c = dark ? c : new Color((byte)(c.B * 1.1f), (byte)(c.G * 1.1f), (byte)(c.B * 1.1f), (byte)175);
        DrawRectangle(x, y, width, 1, (y & 1) == 0 ? c : new Color(c.R, c.G, c.B, (byte)255));
    }
}
static void RenderAllocations(MemorySection msBank)
{
    if (msBank.Label.Contains("RAM_USAGE")) return;

    var x = msBank.Address % 0x100;
    var y = msBank.Address / 0x100;
    var width = msBank.Size + x < 0x100
        ? msBank.Size
        : 0x100 - x;
    var c = msBank.Col;
    DrawRectangle(x, y, width, 1, (y & 1) == 0 ? msBank.Col : new Color(c.R, c.G, c.B, (byte)180));
    var tw = msBank.Size - width;
    int cntr = 0;
    while (tw / 0x100 > 0)
    {
        ++cntr;
        DrawRectangle(0, y + cntr, 0x100, 1, ((y + cntr) & 1) == 0 ? msBank.Col : new Color(c.R, c.G, c.B, (byte)180));
        tw -= 0x100;
    }
    if (tw > 0)
    {
        ++cntr;
        DrawRectangle(0, y + cntr, tw, 1, ((y + cntr) & 1) == 0 ? msBank.Col : new Color(c.R, c.G, c.B, (byte)180));
    }
}
void DrawPopupInfoV2()
{
    if (_OldAddress != _SelectedAddress)
    {
        _OldAddress = _SelectedAddress;
       

        if (_SelectedBank == 0)
        {
            var register = AllRegister.SelectMany(x => x.LayoutList).FirstOrDefault(y => y.Address == _SelectedAddress);
            if (register != null)
            {
                _StringPopup = CreateRegisterText(register);
                DrawPupup();
                return;
            }
        }
        var sectionsByBank = _memorySections.Where(x => x.Bank == _SelectedBank).ToArray();
        _currenMemorySection = sectionsByBank.LastOrDefault(x =>
        _SelectedAddress >= x.Address && _SelectedAddress <= x.Address + x.Size);

        if (_currenMemorySection != null)
        {
            _StringPopup = CreateMemorySectionText(_currenMemorySection);
        }
        else
        {

            _StringPopup = CreateEmptyMemoryMapText();
        }
    }
    DrawPupup();
}

void DrawPupup()
{
    var mp = GetMousePosition();
    if (!string.IsNullOrEmpty(_StringPopup))
    {
        var dimension = MeasureText(_StringPopup, _fontSize);
        int numLines = _StringPopup.Split('\n').Length;
        var xhalf = mp.X - (dimension >> 1);
        int x = (int)Math.Clamp(xhalf, 0, GetScreenWidth() - dimension - 10);

        var tx = (int)Math.Clamp(xhalf + 10, 10, GetScreenWidth() - dimension);
        var ty = (int)mp.Y + 15;

        var c = _currenMemorySection?.Col ?? Color.Black;
        DrawRectangle(x, (int)mp.Y + 10, dimension + 20, (_fontSize * numLines) + 20, new Color(15, 15, 15, 200));
        DrawRectangle(x, (int)mp.Y + (_fontSize * numLines) + 20, dimension + 20, 15, new Color(c.R, c.G, c.B, (byte)100));
        DrawText(_StringPopup, tx, ty, _fontSize, Color.White);
    }
}

void KeyboardTextMode()
{
    int key = Raylib.GetCharPressed();
    while (key > 0)
    {
        if (char.IsAsciiLetterOrDigit((char)key) || char.IsPunctuation((char)key) || char.IsWhiteSpace((char)key))
        {
            _inputText.Insert(_cursorSearchPosition, (char)key);
            _cursorSearchPosition++;
            FilterLabelList();
        }
        key = Raylib.GetCharPressed();
    }

    if ((IsKeyPressed(KeyboardKey.Backspace) || IsKeyPressedRepeat(KeyboardKey.Backspace)) && _cursorSearchPosition > 0)
    {
        _inputText.Remove(_cursorSearchPosition - 1, 1);
        --_cursorSearchPosition;
        FilterLabelList();
    }
    if ((IsKeyPressed(KeyboardKey.Delete) || IsKeyPressedRepeat(KeyboardKey.Delete)) && _cursorSearchPosition < _inputText.Length)
    {
        _inputText.Remove(_cursorSearchPosition, 1);
        FilterLabelList();
    }
    if ((IsKeyPressed(KeyboardKey.Left) || IsKeyPressedRepeat(KeyboardKey.Left)) && _cursorSearchPosition > 0)
    {
        --_cursorSearchPosition;
    }
    if ((IsKeyPressed(KeyboardKey.Right) || IsKeyPressedRepeat(KeyboardKey.Right)) && _cursorSearchPosition < _inputText.Length)
    {
        ++_cursorSearchPosition;
    }
    if (IsKeyPressed(KeyboardKey.Left) && IsKeyDown(KeyboardKey.LeftControl))
    {
        _cursorSearchPosition = 0;
    }
    if (IsKeyPressed(KeyboardKey.Right) && IsKeyDown(KeyboardKey.LeftControl))
    {
        _cursorSearchPosition = _inputText.Length;
    }
    if (IsKeyPressed(KeyboardKey.Down) || IsKeyPressedRepeat(KeyboardKey.Down))
    {
        var entryCount = Math.Min(10, LabelListFiltered.Count);
        _cursorSuggestionPosition = Math.Min(entryCount - 1, _cursorSuggestionPosition + 1);
    }
    if (IsKeyPressed(KeyboardKey.Up) || IsKeyPressedRepeat(KeyboardKey.Up))
    {
        _cursorSuggestionPosition = Math.Max(0, _cursorSuggestionPosition - 1);
    }
    if (IsKeyPressed(KeyboardKey.Tab))
    {
        var entryGT0 = Math.Min(10, LabelListFiltered.Count) > 0;
        if (entryGT0)
        {
            string res = LabelListFiltered[_cursorSuggestionPosition];
            _inputText.Clear();
            _inputText.Append(res);
            _cursorSearchPosition = _inputText.Length;
        }
    }
}

void FilterLabelList()
{
    LabelListFiltered.Clear();
    if (_inputText.Length == 0) return;
    LabelListFiltered.AddRange(LabelList.Where(x => x.ToLower().Contains(_inputText.ToString().ToLower())));
    LabelListFiltered.AddRange(LabelListDynamic.Where(x => x.ToLower().Contains(_inputText.ToString().ToLower())));
    _cursorSuggestionPosition = 0;
}

string CreateRegisterText(RegisterSection register)
{
        _sbtextBox.Clear();
    var layout = GetLayout(_currentLayout, _SelectedBank, _SelectedAddress);
    if (layout != null)
        _sbtextBox.AppendLine($"Region:{layout.Label.PadLeft(21)}");
    var l = Math.Max(20, register.Label.Length);
    _sbtextBox.AppendLine($"Name:{register.Label.PadLeft(l + 6)}");
    _sbtextBox.AppendLine($"Short:{register.LabelShort.PadLeft(l + 5)}");
    _sbtextBox.AppendLine($"Address:{register.Address.ToString("X4").PadLeft(l + 3)}");
    if (!string.IsNullOrEmpty(register.Style))
        _sbtextBox.AppendLine($"Style:{register.Style.PadLeft(l + 5)}");
    if (!string.IsNullOrEmpty(register.Timing))
        _sbtextBox.AppendLine($"Timing:{register.Timing.PadLeft(l + 4)}");
    if (!string.IsNullOrEmpty(register.Access))
        _sbtextBox.Append($"Access:{register.Access.PadLeft(l + 4)}");
    return _sbtextBox.ToString().Replace("\r", "");
}

string CreateEmptyMemoryMapText()
{
    var l = 20;
    _sbtextBox.Clear();
    var layout = GetLayout(_currentLayout, _SelectedBank, _SelectedAddress);
    if (layout != null)
        _sbtextBox.AppendLine($"Region:{layout.Label.PadLeft(21)}");

    _sbtextBox.AppendLine($"Bank:{_SelectedBank.ToString("X2").PadLeft(l + 3)}");
    _sbtextBox.Append($"Address:{_SelectedAddress.ToString("X4").PadLeft(l)}");
    return _sbtextBox.ToString().Replace("\r", "");
}


string CreateMemorySectionText(MemorySection _currenMemorySection)
{
    _sbtextBox.Clear();
    var layout = GetLayout(_currentLayout, _SelectedBank, _SelectedAddress);
    if (layout != null)
        _sbtextBox.AppendLine($"Region:{layout.Label.PadLeft(21)}");
    var l = Math.Max(20, _currenMemorySection.Label.Length);
    _sbtextBox.AppendLine($"Label:{_currenMemorySection.Label.PadLeft(l + 2)}");
    _sbtextBox.AppendLine($"Bank:{_currenMemorySection.Bank.ToString("X2").PadLeft(l + 3)}");
    _sbtextBox.AppendLine($"Address:{_currenMemorySection.Address.ToString("X4").PadLeft(l)}");
    _sbtextBox.Append($"Size:{_currenMemorySection.Size.ToString("X4").PadLeft(l + 3)}");
    return _sbtextBox.ToString().Replace("\r", "");
}

static RomSection? GetLayout(LabeledSection<RomSection>? _currentLayout, byte _SelectedBank, ushort _SelectedAddress)
{
    if (_currentLayout == null) return null;
    return _currentLayout?.LayoutList.FirstOrDefault(x =>
  _SelectedBank >= x.BankStart && _SelectedBank <= x.BankEnd &&
  _SelectedAddress >= x.SectionStart && _SelectedAddress <= x.SectionEnd);
}

static void RenderHighlightPixel(HighLightRegister highLight, float amplitude, float frequency, float offset)
{
    var x = highLight.Address % 0x100;
    var y = highLight.Address / 0x100;
    var width = 1;
    var dark = (x & 1) == 0;

    double time = Raylib.GetTime(); // Get elapsed time in seconds
    double sineValue = Math.Sin(2 * Math.PI * frequency * time); // Sine wave between -1 and 1
    byte modulatedByte = (byte)(amplitude * sineValue + offset); // Scale to [0, 255]

    DrawCircleV(new Vector2(x+.5f , y + .5f), Math.Min(4,modulatedByte>>4), new Color((byte)150, (byte)0, (byte)0,modulatedByte>>1));
    DrawRectangle(x, y, width, 1, new Color((byte)255, (byte)255, (byte)255, modulatedByte));
}

static void DrawHighLightBox(int fontSize, HighLightRegister highLight, ref int boxSize, ref int numLines)
{
    numLines = highLight.Text.Split('\n').Length;
    boxSize = MeasureText(highLight.Text, fontSize);
    DrawRectangle(400, 10, boxSize + 20, numLines * fontSize + 20, new Color(15, 15, 15, 175));
    DrawRectangle(400, 10, boxSize + 20, numLines * fontSize + 20, new Color(15, 15, 15, 175));
    DrawText(highLight.Text, 405, 15, fontSize, Color.RayWhite);
}