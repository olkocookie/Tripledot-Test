# Tripledot Tech Art Test - Mobile UI Implementation

**Developer:** Olga  
**Total Development Time:** 18 hours
- UI Layout & Hierarchy: 3h
- Bottom Bar System: 5h
- Settings Popup & Blur: 3h
- Level Complete Screen: 4h
- Responsive Design: 2h
- Documentation & Polish: 1h
- **Unity Version:** 2022.3.62f2  
- **Target Platform:** iOS/Android

## Project Overview

This project demonstrates a flexible mobile game UI implementation featuring:
- Responsive bottom navigation bar with smooth animations
- Settings overlay with blur effects
- Level completion screen with particle systems and sequenced animations
- Adaptive layouts for multiple device aspect ratios
- Performance-optimized rendering with sprite atlases

## Disclaimer

Due to time constraints, final visual polish and animation timing of production quality were not part of the scope as the goal was to focus on showcasing a wide range of various skills.
LevelCompletedScreen has title text imported as images and is not setup properly as TMP once again due to time constraints and since the goal of that part of the task was to focus
on showcasing animation and VFX handling in general. Still, the animation timing and some values require extra polish (especially the text title), but due to time constraints it was
decided to share the draft look.
All animations are done in code due to time constraints and to showcase code-based animation skills and also to demonstrate another viable approach to implementing UI animations, since
there are case studies proving its better overall performance.
I also had an idea of implementing an instant scroll of the pattern for the LevelComplete background, but it required more time to propely cut and then tile the pattern. The original .png did not have a seamless cut and thus, when moving the image on Y axis via Raw Image component, the cut in pattern was very noticeable and the decision has been made to cut this effect for now.

## Key Features Implemented

### Bottom Navigation Bar
- **5-button expandable system** with dynamic Layout Groups
- **Smooth icon elevation animations** (DOTween)
- **Platform expansion effects** on button activation
- **Lock/unlock functionality** with animated width transitions
- **Extensible architecture** - easily add/remove buttons
- **Responsive design** - adapts to narrow screens (aspect < 0.46)

**Scripts:**
- `BottomBarView.cs` - Main controller with event system
- `BottomBarButton.cs` - Individual button logic and animations

### Settings Popup
- **Blur overlay effect** using custom shader (`UI/FrostedGlass`)
- **Smooth show/hide animations** with fade and scale
- **Darkened background** for focus
- **Optimized 4-sample blur** for mobile performance

**Scripts:**
- `SettingsPopup.cs` - Popup controller with animation sequencing
- `SettingsButtonHandler.cs` - Trigger from main UI

**Shader:**
- `Assets/Shaders/FrostedGlass.shader` - Custom UI blur implementation

### Level Completed Screen
- **Sequenced animation system** with configurable timing
- **Particle systems integration:**
  - One-shot burst particles (confetti)
  - Looped ambient particles (sparkles)
  - Proper UI sorting layer management
- **Smooth screen transitions** with cross-fades
- **Responsive layout** tested on multiple aspect ratios

**Scripts:**
- `LevelCompletedScreenController.cs` - Animation handler
- `TestLevelComplete.cs` - Demo trigger button

### Responsive Design System
- **SafeArePlugin** - Free plugin from Unity store that handles notches/safe areas with Editor simulation
- **AdaptiveBackground.cs** - Multi-device background scaling (16:9, 19.5:9, 4:3)
- **TopBarResponsive.cs** - Adaptive UI element scaling for narrow screens

**Supported Devices:**
- iPhone (16:9, 19.5:9, 21:9)
- iPad (4:3)
- Android (various aspect ratios)

## Architecture & Organization

### Naming Conventions
- **UI Hierarchy:** Descriptive names (e.g., `BottomBar`, `SettingsPopup`, `Star_Big`)
- **Containers:** `Name_Container` pattern for Layout Groups
- **Sprites:** Original asset names preserved for easy identification
- **Scripts:** PascalCase with descriptive, purpose-driven names

## Technical Highlights

### Performance Optimizations
 -  **Sprite Atlases** - Reduced draw calls by batching UI sprites
 -  **Single Canvas architecture** - Minimal canvas hierarchy (one main + nested for overlays only)
 -  **Optimized blur shader** - 4-sample blur vs 9-sample (40% cheaper)
 -  **Texture compression** - All UI textures compressed for mobile
 -  **DOTween recycling** - Proper tween cleanup to avoid memory leaks

### Extensibility
- **Layout Groups** - Easy to add/remove UI elements
- **Scriptable timing** - animation delays/durations in Inspector
- **Modular particle systems** - Swappable particle prefabs

## Setup Instructions

### Prerequisites
- Unity 2022.3.x or later
- DOTween (Free) - Import from Asset Store
- TextMeshPro - Auto-imported with Unity

### First Time Setup
1. Open `Scenes/TechArt_Test.unity`
2. Ensure DOTween is imported and initialized
3. Press Play - everything should work out of the box!

### Testing Features
- **Bottom Bar:** Click different icons to see transitions
- **Settings:** Click gear icon (top-right) to open settings
- **Level Complete:** Click "Test Level Complete" button in scene
- **Device Testing:** Change Game View aspect ratio to test responsiveness

## Known Issues & Future Improvements

### Known Issues (Not Addressed - Time Constraint)

1. **Platform Slide Animation**
   - **Issue:** Bottom bar button platforms don't slide horizontally during activation
   - **Cause:** Animation timing conflicts with DOTween sequence
   - **Workaround:** Disabled slide animation; platforms only expand/fade
   - **Fix Time:** ~30 min to debug tween order
   - **Impact:** Visual only, functionality works perfectly

2. **First Click Delay Pattern**
   - **Issue:** Required two-click workaround for popup activation in earlier iterations
   - **Cause:** OnEnable/SetActive timing edge case
   - **Solution:** Resolved by matching SettingsPopup pattern (OnEnable handles all setup)
   - **Status:** ✅ Fixed in final version

3. **Text Localization Preparation**
   - **Issue:** No localization system implemented
   - **Approach:** Per assignment, created base text prefabs for easy component attachment in the future
   - **Currently missing:**
     - Font atlas generation for Japanese/CJK characters (e.g. Noto Sans CJK)
     - TextMeshPro fallback font system
   - **Fix Time:** ~2-3 hours for full implementation

4. **Memory Profiling**
   - **Issue:** No deep profiling performed on device
   - **Reason:** Time constraint + no access to physical device
   - **Assumption:** Based on draw call reduction and atlas usage, should perform well on mid-range devices

5. **Particle Pooling**
   - **Issue:** Particles instantiate fresh each time (no object pooling)
   - **Reason:** Single screen in demo - minimal performance impact
   - **Fix:** Implement ParticlePool system
   - **Fix Time:** ~45 min

6. **Animation Parameter Tuning**
   - **Issue:** Animation timings/easings are subjective and not extensively playtested
   - **Reason:** Most parameters exposed in Inspector but limited time for iteration
   - **Fix Time:** ~30 min tweaking per screen

7. **Proper Code Refactor**
   - **Issue:** some scripts still have "magic numbers" instead of having every single parameter exposed in the Inspector, controller scripts handle everything and some logic decoupling is required,
   i.e. one script manages state, another one handles all animations and the other updates UI elements
   - **Reason:** Time constraint and the focus of this assignment was to showcase a variety of skills, with code not being its main focus
   - **Fix Time:** ~2-3 hours for fully clean code

## Dependencies

- **DOTween** (Free) - https://assetstore.unity.com/packages/tools/animation/dotween-hotween-v2-27676
- **TextMeshPro** - Included with Unity
- **Unity UI** - Built-in Unity package

## License

Created for Tripledot Tech Art Test - 2026
